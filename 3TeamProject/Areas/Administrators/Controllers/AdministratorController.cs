using _3TeamProject.Areas.Administrators.Data;
using _3TeamProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Security.Cryptography;

namespace _3TeamProject.Areas.Administrators.Controllers
{
    [Area("Administrators")]
    
    public class AdministratorController : Controller
    {
        private readonly _3TeamProjectContext _context;
        private readonly IConfiguration _env;

        public AdministratorController(_3TeamProjectContext Context, IConfiguration env)
        {
            _context = Context;
            _env=env;
        }

        public async Task<IActionResult> GetAllAdmin()
        {
            
            //TODO get data by Admin
            return Ok(_context.Administrators.ToList());
        }
        [Authorize(Roles ="Suppliers")]
        public async Task<IActionResult> Register([FromBody] AdminRegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Account == request.Account))
            {
                return BadRequest("帳號已經存在");
            }

            using (var hmac = new HMACSHA512())
            {
                var passwordSalt = hmac.Key;
                var passwordHsah = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));
                var verifyToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
                Administrator admin = new Administrator
                {
                    AdministratorName = request.AdministratorName,
                    PhoneNumber = request.PhoneNumber,
                    IdNavigation = new User
                    {
                        Account = request.Account,
                        Email = request.Email,
                        PasswordHash = passwordHsah,
                        PasswordSalt = passwordSalt,
                        VerficationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
                        Roles = request.Roles
                    }
                };

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

                _context.Administrators.Add(admin);
                await _context.SaveChangesAsync();
                return Ok("註冊成功，請等待驗證信件");
            }
        }
    }
}
