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
using _3TeamProject.Services;

namespace _3TeamProject.Controllers
{
    public class UserController : Controller
    {
        private readonly _3TeamProjectContext _context;
        private readonly IConfiguration _env;

        public UserController(_3TeamProjectContext context, IConfiguration env)
        {
            _context = context;
            _env=env;
        }
        public async Task<IActionResult> Login([FromBody] LoginReuqest request)
        {
            var user = await _context.Users.Include(u => u.RolesNavigation).FirstOrDefaultAsync(u => u.Account == request.Account);
            if (user == null)
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
        
        public async Task<IActionResult> Verify([FromBody] string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerficationToken == token);
            if (user == null)
            {
                return BadRequest("無效的驗證碼");
            }
            if (user.VerfiedAt != null)
            {
                return BadRequest("已驗證過的驗證碼");
            }
            user.VerfiedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok("驗證成功");
        }

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

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("dotnettgm102@gmail.com", "帳號驗證碼");
                mail.To.Add("dotnettgm102@gmail.com");
                mail.Priority = MailPriority.Normal;
                mail.Subject = "帳號驗證碼";
                mail.Body = $"<a href=\"https://localhost:7190/User/Verify\"  value=\"{verifyToken}\">帳號驗證碼</a>";
                mail.IsBodyHtml = true;
                SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);
                MySmtp.Credentials = new System.Net.NetworkCredential(_env["mail:Account"], _env["mail:Password"]);
                MySmtp.EnableSsl = true;
                MySmtp.Send(mail);
                MySmtp = null;
            };

            await _context.SaveChangesAsync();
            return Ok("請等待驗證信件");
        }
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
                return Ok("密碼重設成功");
            }
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Logout");
            //return RedirectToAction("Login", "home");
        }
    }
}
