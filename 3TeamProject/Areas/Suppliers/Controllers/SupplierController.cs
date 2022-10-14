using _3TeamProject.Areas.Sppliers.Data;
using _3TeamProject.Data;
using _3TeamProject.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.Json;

namespace _3TeamProject.Areas.Sppliers.Controllers
{
    [Area("Suppliers")]
    public class SupplierController : Controller
    {
        private readonly _3TeamProjectContext _context;

        public SupplierController(_3TeamProjectContext Context)
        {
            _context = Context;
        }
        public async Task<IActionResult> GetSupplier()
        {
            //TODO get data by login supplier
            return Ok();
        }
        public async Task<IActionResult> GetAllSupplier()
        {
            //TODO get data by Admin
            return Ok();
        }
        public async Task<IActionResult> Register([FromBody]SupplierRegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Account == request.Account))
            {
                return BadRequest("帳號已經存在");
            }
            using (var hmac = new HMACSHA512())
            {
                var passwordSalt = hmac.Key;
                var passwordHsah = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));
                Supplier supplier = new Supplier
                {
                    ContactName = request.ContactName,
                    CompanyName = request.CompanyName,
                    TaxId = request.TaxId,
                    Fax = request.Fax,
                    CellPhoneNumber = request.CellPhoneNumber,
                    SupplierPhoneNumber = request.SupplierPhoneNumber,
                    SupplierPostalCode = request.SupplierPostalCode,
                    SupplierCountry = request.SupplierCountry,
                    SupplierCity = request.SupplierCity,
                    SupplierAddress = request.SupplierAddress,
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
                _context.Suppliers.Add(supplier);
                await _context.SaveChangesAsync();
                return Ok("註冊成功，請等待驗證信件");
            }
        }

        public async Task<IActionResult> Login([FromBody]SupplierLoginReuqest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Account == request.Account);
            if (user == null) {
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
            return Ok("登入成功");
        }
        //TODO 新增寄送Token給使用者作驗證
        public async Task<IActionResult> Verify([FromBody]string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerficationToken == token);
            if (user == null) {
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
        //TODO 新增重設密碼寄送Token給使用者作驗證
        public async Task<IActionResult> ForgotPassword([FromBody]string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return BadRequest("帳號不存在");
            }
            user.PasswordResetToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
            user.ResetTokenExpires = DateTime.Now.AddMinutes(30);
            await _context.SaveChangesAsync();
            return Ok("請等待驗證信件");
        }
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordRequest request)
        {
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
                user.PasswordSalt = passwordSalt;
                user.PasswordHash = passwordHsah;
                user.PasswordResetToken = null;
                user.ResetTokenExpires = null;
                await _context.SaveChangesAsync();
                return Ok("密碼重設成功");
            }
        }
        //TODO 新增Cookie驗證
    }
}
