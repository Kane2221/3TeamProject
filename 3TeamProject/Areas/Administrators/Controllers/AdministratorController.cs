using _3TeamProject.Areas.Administrators.Data;
using _3TeamProject.Areas.Sppliers.Data;
using _3TeamProject.Data;
using _3TeamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace _3TeamProject.Areas.Administrators.Controllers
{
    [Area("Administrators")]
    public class AdministratorController : Controller
    {
        private readonly _3TeamProjectContext _context;

        public AdministratorController(_3TeamProjectContext Context)
        {
            _context = Context;
        }

        public async Task<IActionResult> GetAllAdmin()
        {
            
            //TODO get data by Admin
            return Ok(_context.Administrators.ToList());
        }
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
                _context.Administrators.Add(admin);
                await _context.SaveChangesAsync();
                return Ok("註冊成功，請等待驗證信件");
            }
        }
    }
}
