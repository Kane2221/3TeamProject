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

namespace _3TeamProject.Controllers.Api
{
    [Route("Users/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly _3TeamProjectContext _context;
        private readonly IConfiguration _config;

        public UserApiController(_3TeamProjectContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        //登入帳號
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginReuqest request)
        {
            var user = await _context.Users.Include(u => u.RolesNavigation)
                .FirstOrDefaultAsync(u => u.Account == request.Account);
            if (user == null)
            {
                return BadRequest("帳號不存在");
            }
            //判斷會員帳號已刪除的情況不能登入
            var member = _context.Members.Include(m => m.User)
                .Where(m => m.User.Account == request.Account).FirstOrDefault();
            if (member == null || member.MemberStatusId == 4)
            {
                return BadRequest("帳號不存在");
            }
            //判斷廠商帳號已刪除的情況不能登入
            var supplier = _context.Suppliers.Include(m => m.User)
                .Where(m => m.User.Account == request.Account).FirstOrDefault();
            if (supplier == null || supplier.SupplierStatusId == 3)
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
            //return RedirectToAction("index", "home");
        }
        [HttpPost("Verify")]
        //驗證帳號
        public async Task<IActionResult> Verify([FromBody] string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerficationToken == token);
            var member = _context.Members.Include(m => m.User)
                .Where(m => m.User.VerficationToken == token).FirstOrDefault();
            var supplier = _context.Suppliers.Include(m => m.User)
                .Where(m => m.User.VerficationToken == token).FirstOrDefault();
            if (user == null)
            {
                return BadRequest("無效的驗證碼");
            }
            if (user.VerfiedAt != null)
            {
                return BadRequest("已驗證過的驗證碼");
            }
            user.VerfiedAt = DateTime.Now;
            member.MemberStatusId = 2;
            supplier.SupplierStatusId = 1;
            await _context.SaveChangesAsync();
            return Ok("驗證成功");
        }
        [HttpPost("ForgotPassword")]
        //忘記密碼
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
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
                mail.From = new MailAddress("dotnettgm102@gmail.com", "帳號驗證碼");
                mail.To.Add("dotnettgm102@gmail.com");
                mail.Priority = MailPriority.Normal;
                mail.Subject = "帳號驗證碼";
                mail.Body = $"<a href=\"{root}\" value=\"{verifyToken}\">帳號驗證碼</a>";
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
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return BadRequest("驗證碼已過期，請重新申請!");
            }
            if (request.Password != request.ComfirmPassword)
            {
                return BadRequest("密碼與確認密碼不一致");
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
    }
}
