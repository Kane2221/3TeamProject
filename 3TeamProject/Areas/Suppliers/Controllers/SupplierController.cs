using _3TeamProject.Areas.Administrators.Data;
using _3TeamProject.Areas.Sppliers.Data;
using _3TeamProject.Areas.Suppliers.Data;
using _3TeamProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;


namespace _3TeamProject.Areas.Sppliers.Controllers
{

    [Route("Suppliers/[controller]")]
    [ApiController]
    public class SupplierController : Controller
    {
        private readonly _3TeamProjectContext _context;
        private readonly IConfiguration _config;
        private readonly IHostEnvironment _env;

        public SupplierController(_3TeamProjectContext Context, IConfiguration config, IHostEnvironment env)
        {
            _context = Context;
            _config = config;
            _env=env;
        }
        [Authorize("Suppliers")]
        [HttpGet("{id}")]
        public IActionResult GetSupplier(int id)
        {
            var UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            if (id != UserId)
            {
                return BadRequest("與登入帳號不符");
            }
            var user = _context.Users.Include(u => u.Suppliers).FirstOrDefault(x => x.UserId == UserId);
            if (user == null)
            {
                return BadRequest("沒有此帳號");
            }
            var supplier = (from u in _context.Users
                            where u.UserId == user.UserId
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
                            }).SingleOrDefault();
            return Ok(supplier);
        }

        [AllowAnonymous]
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] SupplierRegisterViewModel request)
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

                Supplier supplier = new Supplier
                {
                    User = new User
                    {
                        Account = request.Account,
                        Email = request.Email,
                        PasswordHash = passwordHsah,
                        PasswordSalt = passwordSalt,
                        VerficationToken = verifyToken,
                        Roles = request.Roles
                    }
                };

                #region Send Email with verify code (正式再解開註解)
                //TODO 修改寄信的超連結
                //var root = $@"{Request.}User\Verify";
                //using (MailMessage mail = new MailMessage())
                //{
                //    mail.From = new MailAddress("dotnettgm102@gmail.com", "帳號驗證碼");
                //    mail.To.Add("dotnettgm102@gmail.com");
                //    mail.Priority = MailPriority.Normal;
                //    mail.Subject = "帳號驗證碼";
                //    mail.Body = $"<a href=\"{root}\"  value=\"{verifyToken}\">帳號驗證碼</a>"; 
                //    mail.IsBodyHtml = true;
                //    SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);
                //    MySmtp.UseDefaultCredentials = false;
                //    MySmtp.Credentials = new System.Net.NetworkCredential(_config["mail:Account"], _config["mail:Password"]);
                //    MySmtp.EnableSsl = true;
                //    MySmtp.Send(mail);
                //    MySmtp = null;
                //};
                #endregion

                //_context.Suppliers.Add(supplier);
                _context.Suppliers.Add(supplier).CurrentValues.SetValues(request);
                await _context.SaveChangesAsync();
                return Ok("註冊成功，請等待驗證信件");
            }
        }

        [Authorize("Suppliers")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SupplierUpdateViewModel request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }
            var UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            if (id != UserId)
            {
                return BadRequest("與登入帳號不符");
            }
            var supplier = _context.Suppliers.Include(s => s.User).Where(s => s.UserId == id)
                .Select(s => s).SingleOrDefault();

            using (var hmac = new HMACSHA512())
            {
                var passwordSalt = hmac.Key;
                var passwordHsah = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));
                supplier.ContactName = request.ContactName;
                supplier.CompanyName = request.CompanyName;
                supplier.TaxId = request.TaxId;
                supplier.Fax = request.Fax;
                supplier.CellPhoneNumber = request.CellPhoneNumber;
                supplier.SupplierPhoneNumber = request.SupplierPhoneNumber;
                supplier.SupplierPostalCode = request.SupplierPostalCode;
                supplier.SupplierCountry = request.SupplierCountry;
                supplier.SupplierCity = request.SupplierCity;
                supplier.SupplierAddress = request.SupplierAddress;
                supplier.User.PasswordHash = passwordHsah;
                supplier.User.PasswordSalt = passwordSalt;
                supplier.User.Email = request.Email;
                _context.Suppliers.Update(supplier);
                await _context.SaveChangesAsync();
            }
            return Ok("修改成功!");
        }
        [Authorize("Suppliers")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            if (id != UserId)
            {
                return BadRequest("與登入帳號不符");
            }
            var user = _context.Users.Include(u => u.Suppliers).FirstOrDefault(x => x.UserId == id);
            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok("此帳號已刪除");
        }
        [Authorize("Suppliers")]
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id) //TODO 待測_廠商管理商品
        {
            var UserID = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var products = _context.Products.Include(p => p.ProductStatus).Include(p => p.ProductCategory).Include(p => p.ProductsPictureInfos)
                .Where(p=>p.Supplier.UserId == UserID).Select(p => new GetProductViewModel
                {
                    ProductId = p.ProductId,
                    CategoryName = p.ProductCategory.CategoryName,
                    ProductName = p.ProductName,
                    QuantityPerUnit = p.QuantityPerUnit,
                    ProductUnitPrice = p.ProductUnitPrice,
                    UnitStock = p.UnitStock,
                    UniOnOrder = p.UniOnOrder,
                    ProductRecommendation = p.ProductRecommendation,
                    StatusName = p.ProductStatus.StatusName,
                });
            return Ok(products);
        }
        [Authorize("Suppliers")]
        [HttpPost("Upload")]
        public IActionResult UploadProduct() //TODO 商品上架
        {
            return Ok();
        }
    }
}
