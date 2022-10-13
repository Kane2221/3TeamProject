using _3TeamProject.Areas.Sppliers.Data;
using _3TeamProject.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

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
                    SupplierPhnoeNumber = request.SupplierPhnoeNumber,
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
                return Ok("註冊成功");
            }
        }

        public async Task<IActionResult> Login([FromBody] SupplierLoginReuqest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Account == request.Account);
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
            hmac.Dispose();

            return Ok("登入成功");
        }
        public async Task<IActionResult> Verify(string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerficationToken == token);
            if (user == null)
            {
                return BadRequest("失效的驗證碼");
            }
            user.VerfiedAt = DateTime.Now;
            await _context.SaveChangesAsync();



            return Ok("驗證成功");
        }
    }
}
