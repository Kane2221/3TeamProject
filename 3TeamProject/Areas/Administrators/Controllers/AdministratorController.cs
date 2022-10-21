using _3TeamProject.Areas.Administrators.Data;
<<<<<<< HEAD
=======
using _3TeamProject.Areas.Suppliers.Data;
>>>>>>> e1af6f3e8a20b9717349e0b46dfb6f09d329bbe6
using _3TeamProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using System.Net.Mail;
=======
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Security.Claims;
>>>>>>> e1af6f3e8a20b9717349e0b46dfb6f09d329bbe6
using System.Security.Cryptography;
using System.Security.Principal;

namespace _3TeamProject.Areas.Administrators.Controllers
{
    [Area("Administrators")]
    
    public class AdministratorController : Controller
    {
        private readonly _3TeamProjectContext _context;
<<<<<<< HEAD
        private readonly IConfiguration _env;

        public AdministratorController(_3TeamProjectContext Context, IConfiguration env)
        {
            _context = Context;
            _env=env;
=======
        private readonly IConfiguration _config;

        public AdministratorController(_3TeamProjectContext Context, IConfiguration config)
        {
            _context = Context;
            _config = config;
>>>>>>> e1af6f3e8a20b9717349e0b46dfb6f09d329bbe6
        }

        [Authorize(Roles = ("Administrator, ChiefAdministrator, SuperAdministrator"))]
        public IActionResult GetAllAdmins() //權限Administrator只能看見同權限的清單，更高權限可以看見所有人清單
        {
<<<<<<< HEAD
            
            //TODO get data by Admin
            return Ok(_context.Administrators.ToList());
        }
        [Authorize(Roles ="Suppliers")]
        public async Task<IActionResult> Register([FromBody] AdminRegisterRequest request)
=======
            var UserRole = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            if (UserRole == "Administrator")
            {
                var admin = _context.Administrators.Include(u => u.User)
                        .Where(u => u.User.Roles == 5).Select(u => new AdminGetViewModel
                        {
                            Account = u.User.Account,
                            Email = u.User.Email,
                            Roles = u.User.RolesNavigation.RoleName,
                            AdministratorName = u.AdministratorName,
                            PhoneNumber = u.PhoneNumber
                        });
                return Ok(admin);
            }
            else if (UserRole == "ChiefAdministrator")
            {
                var adminChief = _context.Administrators.Include(u => u.User)
                    .Where(u => u.User.Roles != 3).Select(u => new AdminGetViewModel
                    {
                        Account = u.User.Account,
                        Email = u.User.Email,
                        Roles = u.User.RolesNavigation.RoleName,
                        AdministratorName = u.AdministratorName,
                        PhoneNumber = u.PhoneNumber
                    });
                return Ok(adminChief);
            }
            var adminSuper = _context.Administrators.Include(u => u.User)
                    .Select(u => new AdminGetViewModel
                    {
                        Account = u.User.Account,
                        Email = u.User.Email,
                        Roles = u.User.RolesNavigation.RoleName,
                        AdministratorName = u.AdministratorName,
                        PhoneNumber = u.PhoneNumber
                    });
            return Ok(adminSuper);            
        }

        public async Task<IActionResult> Register([FromBody] AdminRequstViewModel request)
>>>>>>> e1af6f3e8a20b9717349e0b46dfb6f09d329bbe6
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }
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
<<<<<<< HEAD

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
=======
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
>>>>>>> e1af6f3e8a20b9717349e0b46dfb6f09d329bbe6

                _context.Administrators.Add(admin);
                await _context.SaveChangesAsync();
                return Ok("註冊成功，請等待驗證信件");
            }
        }
<<<<<<< HEAD
=======
        [Authorize(Roles = ("Administrator, ChiefAdministrator, SuperAdministrator"))]
        public async Task<IActionResult> Update(int? id, [FromBody] AdminUpdateViewModel request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }

            var admin = _context.Administrators.Include(a => a.User)
                    .Where(a => a.UserId == id).Select(a => a).SingleOrDefault();
            if (admin == null)
            {
                return BadRequest("此帳號不存在");
            }
            using (var hmac = new HMACSHA512())
            {
                var passwordSalt = hmac.Key;
                var passwordHsah = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));
                admin.User.Email = request.Email;
                admin.AdministratorName = request.AdministratorName;
                admin.PhoneNumber = request.PhoneNumber;
                admin.User.PasswordSalt = passwordSalt;
                admin.User.PasswordHash = passwordHsah;
                _context.Administrators.Update(admin);
                await _context.SaveChangesAsync();
            }
            return Ok("修改成功!");
        }
        [Authorize(Roles ="SuperAdminstrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = _context.Users.Include(u => u.Administrators).FirstOrDefault(x => x.UserId == id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("此帳號已刪除");
        }
>>>>>>> e1af6f3e8a20b9717349e0b46dfb6f09d329bbe6
    }
}
