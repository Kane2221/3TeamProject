using _3TeamProject.Areas.Administrators.Data;
using _3TeamProject.Areas.Suppliers.Data;
using _3TeamProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;

namespace _3TeamProject.Areas.Administrators.Controllers
{
    [Authorize(Roles = ("Administrator, ChiefAdministrator, SuperAdministrator"))]
    [Route("Administrators/[controller]")]
    [ApiController]
    public class AdministratorController : Controller
    {
        private readonly _3TeamProjectContext _context;
        private readonly IConfiguration _config;

        public AdministratorController(_3TeamProjectContext Context, IConfiguration config)
        {
            _context = Context;
            _config = config;
        }

        [HttpGet("GetAllAdmins")]//權限Administrator只能看見同權限以下的清單，更高權限可以看見所有人清單
        public IActionResult GetAllAdmins()
        {
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
                var admin = _context.Administrators.Include(u => u.User)
                    .Where(u => u.User.Roles != 3).Select(u => new AdminGetViewModel
                    {
                        Account = u.User.Account,
                        Email = u.User.Email,
                        Roles = u.User.RolesNavigation.RoleName,
                        AdministratorName = u.AdministratorName,
                        PhoneNumber = u.PhoneNumber
                    });
                return Ok(admin);
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
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] AdminRegisterViewModel request)
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

                _context.Administrators.Add(admin);
                await _context.SaveChangesAsync();
                return Ok("註冊成功，請等待驗證信件");
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update(int? id, [FromBody] AdminUpdateViewModel request)
        {
            var UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            if (id != UserId)
            {
                return BadRequest("與登入帳號不符");
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }

            var admin = _context.Administrators.Include(a => a.User)
                    .Where(a => a.UserId == id).Select(a => a).SingleOrDefault();
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
        [HttpDelete]
        [Authorize(Roles = "SuperAdministrator")]
        public async Task<IActionResult> Delete(int id)
        {
            User user = _context.Users.Include(u => u.Administrators).FirstOrDefault(x => x.UserId == id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("此帳號已刪除");
        }
        [HttpGet("GetAllSuppliers")]
        public IActionResult GetAllSuppliers()
        {
            var supplier = from u in _context.Users
                           join s in _context.Suppliers
                           on u.UserId equals s.UserId
                           select new SupplierGetViewModel
                           {
                               Account = u.Account,
                               Email = u.Email,
                               RoleName = u.RolesNavigation.RoleName,
                               ContactName = s.ContactName,
                               CompanyName = s.CompanyName,
                               TaxId = s.TaxId,
                               Fax = s.Fax,
                               CellPhoneNumber = s.CellPhoneNumber,
                               SupplierPhoneNumber = s.SupplierPhoneNumber,
                               SupplierPostalCode = s.SupplierPostalCode,
                               SupplierCountry = s.SupplierCountry,
                               SupplierCity = s.SupplierCity,
                               SupplierAddress = s.SupplierAddress
                           };
            return Ok(supplier);
        }
        [HttpGet]
        public IActionResult GetAllProducts() // TODO 商品管理頁
        {
            return Ok();
        }
        [HttpGet]
        public IActionResult GetAllOrders() // TODO 訂單管理頁
        {
            return Ok();
        }
        [HttpGet]
        public IActionResult GetAllSightseeing() // TODO 景點管理頁
        {
            return Ok();
        }
        [HttpPost]
        public IActionResult UploadSightseeing() // TODO 景點上傳頁
        {
            return Ok();
        }
        public IActionResult GetAllActivities() // TODO 社群活動管理頁
        {
            return Ok();
        }
    }
}
