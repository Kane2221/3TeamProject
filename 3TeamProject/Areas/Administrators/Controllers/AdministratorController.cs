using _3TeamProject.Areas.Administrators.Data;
using _3TeamProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;

namespace _3TeamProject.Areas.Administrators.Controllers
{
    [Area("Administrators")]
    
    public class AdministratorController : Controller
    {
        private readonly _3TeamProjectContext _context;
        private readonly IConfiguration _config;

        public AdministratorController(_3TeamProjectContext Context, IConfiguration config)
        {
            _context = Context;
            _config = config;
        }

        [Authorize(Roles =("Administrator, ChiefAdministrator, SuperAdministrator"))]
        public IActionResult GetAllAdmins() //權限Administrator只能看見同權限的清單，更高權限可以看見所有人清單
        {
            //var UserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value;
            var UserRole = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            //if (UserId == null)
            //{
            //    return BadRequest("未登入");
            //}

            if (UserRole == "Administrator")
            {
                //var admin = _context.Administrators
                //     .Include(x => x.IdNavigation)
                //     .Include(x => x.IdNavigation.RolesNavigation)
                //     .Where(x => x.Id == x.IdNavigation.Id && x.IdNavigation.Roles == 5)
                //     .Select(a => new AdminGetViewModel
                //     {
                //         Account = a.IdNavigation.Account,
                //         Email = a.IdNavigation.Email,
                //         Roles = a.IdNavigation.RolesNavigation.RoleName,
                //         AdministratorName = a.AdministratorName,
                //         PhoneNumber = a.PhoneNumber,
                //     });
                var admin = from u in _context.Users
                            join a in _context.Administrators
                            on u.UserId equals a.UserId
                            where u.Roles == 5
                            select new AdminGetViewModel
                            {
                                Account = u.Account,
                                Email = u.Email,
                                Roles = u.RolesNavigation.RoleName,
                                AdministratorName = a.AdministratorName,
                                PhoneNumber = a.PhoneNumber
                            };
                return Ok(admin);
            }
            var adminAuth = from u in _context.Users
                            join a in _context.Administrators
                            on u.UserId equals a.UserId
                            select new AdminGetViewModel
                            {
                                Account = u.Account,
                                Email = u.Email,
                                Roles = u.RolesNavigation.RoleName,
                                AdministratorName = a.AdministratorName,
                                PhoneNumber = a.PhoneNumber
                            };

            //if (user == null)
            //{
            //    return BadRequest("沒有此帳號");
            //}
            return Ok(adminAuth);
            //return Ok(_context.Users.Include(a => a.Administrators).ToList());
        } 
        
        public async Task<IActionResult> Register([FromBody] AdminRequstViewModel request)
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
                    User = new User
                    {
                        Account = request.Account,
                        Email = request.Email,
                        PasswordHash = passwordHsah,
                        PasswordSalt = passwordSalt,
                        VerficationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
                        Roles = request.Roles
                    }
                };
                #region Send Email with verify code (正式再解開註解)
                //using (MailMessage mail = new MailMessage())
                //{
                //    mail.From = new MailAddress("dotnettgm102@gmail.com", "帳號驗證碼");
                //    mail.To.Add("dotnettgm102@gmail.com");
                //    mail.Priority = MailPriority.Normal;
                //    mail.Subject = "帳號驗證碼";
                //    mail.Body = $"<a href=\"https://localhost:7190/User/Verify\"  value=\"{verifyToken}\">帳號驗證碼</a>";
                //    mail.IsBodyHtml = true;
                //    SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);
                //    MySmtp.UseDefaultCredentials = false;
                //    MySmtp.Credentials = new System.Net.NetworkCredential(_config["mail:Account"], _config["mail:Password"]);
                //    MySmtp.EnableSsl = true;
                //    MySmtp.Send(mail);
                //    MySmtp = null;
                //}; 
                #endregion

                _context.Administrators.Add(admin);
                await _context.SaveChangesAsync();
                return Ok("註冊成功，請等待驗證信件");
            }
        }
    }
}
