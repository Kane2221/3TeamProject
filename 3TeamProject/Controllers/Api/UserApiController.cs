using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using _3TeamProject.Models;
using _3TeamProject.Data;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using Microsoft.Win32;

namespace _3TeamProject.Controllers.Api
{
    [Route("Users/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly _3TeamProjectContext _context;
        private readonly IConfiguration _config;
        private readonly IHostEnvironment _env;

        public UserApiController(_3TeamProjectContext context, IConfiguration config, IHostEnvironment env)
        {
            _context = context;
            _config = config;
            _env=env;
        }
        //登入帳號
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var user = await _context.Users.Include(u => u.RolesNavigation)
                .Include(u=>u.Members).Include(u=>u.Suppliers)
                .FirstOrDefaultAsync(u => u.Account == request.Account);
            if (user == null || 
                user.Members.Where(m=>m.MemberStatusId == 4).SingleOrDefault() != null || 
                user.Suppliers.Where(s=>s.SupplierStatusId == 3).SingleOrDefault() != null)
            {
                return BadRequest("帳號不存在");
            }
            var hmac = new HMACSHA512(user.PasswordSalt);
            var computeHsah = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));
            if (!computeHsah.SequenceEqual(user.PasswordHash))
            {
                return BadRequest("密碼錯誤");
            }
            if (user.VerfiedAt == null)
            {
                return BadRequest("未驗證帳號");
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Account),
                new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.RolesNavigation.RoleName)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPricipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPricipal);
            return Ok("登入成功");
        }
        //註冊驗證帳號
        [HttpPost("Verify")]
        public async Task<IActionResult> Verify([FromBody] VerifyDto request)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == request.Token);
            var account = await _context.Users.FirstOrDefaultAsync(a=>a.Account == request.Account);
            if (account == null)
            {
                return BadRequest("帳號錯誤");
            }

            if (user == null)
            {
                return BadRequest("無效的驗證碼");
            }
            if (user.VerfiedAt != null)
            {
                return BadRequest("已驗證過的驗證碼");
            }
            var member = _context.Members.Include(m => m.User)
                .Where(m => m.User.VerificationToken == request.Token).FirstOrDefault();
            var supplier = _context.Suppliers.Include(m => m.User)
                .Where(m => m.User.VerificationToken == request.Token).FirstOrDefault();

            user.VerfiedAt = DateTime.Now;
            if (member != null)
            {
                member.MemberStatusId = 2;
            }
            else if (supplier != null)
            {
                supplier.SupplierStatusId = 1;
            }
            await _context.SaveChangesAsync();
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Account),
                new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.RolesNavigation.RoleName)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPricipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPricipal);
            return Ok("驗證成功");
        }
        [HttpPost("ForgotPassword")]
        //忘記密碼
        public async Task<IActionResult> ForgotPassword([FromBody] FogotPasswordDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return BadRequest("帳號不存在");
            }
            var verifyToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
            user.PasswordResetToken = verifyToken;
            user.ResetTokenExpires = DateTime.Now.AddMinutes(30);

            var root = $@"{Request.Scheme}:/{Request.Host}/User/Verify";
            //TODO 修改寄信的超連結
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("dotnettgm102@gmail.com", "帳號重設驗證碼");
                mail.To.Add(request.Email);
                mail.Priority = MailPriority.Normal;
                mail.Subject = "帳號重設驗證碼";
                mail.Body = $"<h1>請到以下頁面輸入驗證碼 : {verifyToken}</h1>/n " +
                            $"<a href=\"{root}\">帳號重設驗證碼</a>";
                mail.IsBodyHtml = true;
                SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);
                MySmtp.UseDefaultCredentials = false;
                MySmtp.Credentials = new System.Net.NetworkCredential(_config["mail:Account"], _config["mail:Password"]);
                MySmtp.EnableSsl = true;
                MySmtp.Send(mail);
                MySmtp = null;
            };

            await _context.SaveChangesAsync();
            return Ok("請等待驗證信件");
        }
        [HttpPost("ResetPassword")]
        //重設密碼
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return BadRequest("驗證碼錯誤");
            }
            if (user.ResetTokenExpires < DateTime.Now)
            {
                return BadRequest("驗證碼已過期，請重新申請!");
            }
            using (var hmac = new HMACSHA512())
            {
                var passwordSalt = hmac.Key;
                var passwordHsah = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));
                var verifyToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
                user.PasswordSalt = passwordSalt;
                user.PasswordHash = passwordHsah;
                user.PasswordResetToken = null;
                user.ResetTokenExpires = null;
                await _context.SaveChangesAsync();

            }
            return Ok("密碼重設成功");
        }
        //登出
        [Authorize]
        [HttpDelete("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("已登出");
            //return RedirectToAction("Login", "home");
        }
        //使用者上傳圖片
        [Authorize]
        [HttpPost("UploadPicture")]
        public IActionResult UploadPicture([FromForm] UploadPictureDto request)
        {
            var UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var user = _context.Users.FirstOrDefault(x => x.UserId == UserId);
            //為每一張上傳圖片給值及上傳位置
            if (request.UserPicFiles != null)
            {
                    var root = $@"{_env.ContentRootPath}\wwwroot\";
                    var tempRoot = "";
                    if (request.UserPicFiles.FileName.Contains(".jpg") || request.UserPicFiles.FileName.Contains(".png"))
                    {
                        tempRoot = root +"img"+"\\"+"User"+"\\" +"picture"+ "\\" + user.Account;
                    }
                    else
                    {
                        tempRoot = root +"img"+"\\"+"User"+"\\"+"other"+ "\\" + user.Account;
                    }
                    if (!Directory.Exists(tempRoot))
                    {
                        Directory.CreateDirectory(tempRoot);
                    }
                    var path = tempRoot +"\\" + request.UserPicFiles.FileName;
                    request.UserPicFiles.CopyTo(System.IO.File.Create(path));
                    user.PicturePath = "\\" + path.Replace(root, "");
            }
            _context.Users.Update(user);
            _context.SaveChanges();
            return Ok("已上傳成功");
        }
    }
}
