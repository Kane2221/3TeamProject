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
    [Authorize(Roles ="Suppliers")]
    [Route("Suppliers/[controller]")]
    [ApiController]
    public class SupplierApiController : ControllerBase
    {
        private readonly _3TeamProjectContext _context;
        private readonly IConfiguration _config;
        private readonly IHostEnvironment _env;

        public SupplierApiController(_3TeamProjectContext Context, IConfiguration config, IHostEnvironment env)
        {
            _context = Context;
            _config = config;
            _env=env;
        }
        //取得廠商資料by登入帳號
        [HttpGet("GetSupplier")]
        public IActionResult GetSupplier()
        {
            var UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var user = _context.Users.Include(u => u.Suppliers).FirstOrDefault(x => x.UserId == UserId);
            if (user == null)
            {
                return BadRequest("沒有此帳號");
            }
            var supplier = (from u in _context.Users
                            where u.UserId == UserId
                            join s in _context.Suppliers
                            on u.UserId equals s.UserId
                            select new GetSupplierDto
                            {
                                UserId = u.UserId,
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
                                SupplierAddress = s.SupplierAddress,
                                PicturePath = u.PicturePath
                            }).SingleOrDefault();
            return Ok(supplier);
        }
        //廠商註冊
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterSupplierDto request)
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
                    SupplierStatusId = 0,
                    User = new User
                    {
                        Account = request.Account,
                        Email = request.Email,
                        PasswordHash = passwordHsah,
                        PasswordSalt = passwordSalt,
                        VerificationToken = verifyToken,
                        Roles = 2
                    }
                };
                #region Send Email with verify code (正式再解開註解)
                //var root = $@"{Request.Scheme}:/{Request.Host}/Member/Verify";
                ////TODO 修改寄信的超連結
                //using (MailMessage mail = new MailMessage())
                //{
                //    mail.From = new MailAddress("dotnettgm102@gmail.com", "帳號驗證碼");
                //    mail.To.Add(request.Email);
                //    mail.Priority = MailPriority.Normal;
                //    mail.Subject = "帳號驗證碼";
                //    mail.Body = $"<h1>請點選以下連結驗證您的帳號</h1> " +
                //                $"<a href=\"https://localhost:7007/Member/Verify?token={verifyToken}&account={request.Account}\"><h1>帳號驗證碼</h1></a>";
                //    mail.IsBodyHtml = true;
                //    SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);
                //    MySmtp.UseDefaultCredentials = false;
                //    MySmtp.Credentials = new System.Net.NetworkCredential(_config["mail:Account"], _config["mail:Password"]);
                //    MySmtp.EnableSsl = true;
                //    MySmtp.Send(mail);
                //    MySmtp = null;
                //};
                #endregion
                _context.Suppliers.Add(supplier).CurrentValues.SetValues(request);
                await _context.SaveChangesAsync();
                return Ok("註冊成功，請等待管理員之審核信件");
            }
        }
        //廠商修改資料by登入帳號
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSupplierDto request)
        {
            var UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var supplier = _context.Suppliers.Include(s => s.User).Where(s => s.UserId == UserId)
                .Select(s => s).SingleOrDefault();
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }
            if (id != UserId)
            {
                return BadRequest("刪除帳號與登入帳號不符!");
            }
            if (supplier == null)
            {
                return BadRequest("無此帳號!");
            }
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
        //廠商刪除自己帳號by登入帳號
        [HttpDelete]
        public IActionResult Delete()
        {
            var UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var user = _context.Users.Include(u => u.Suppliers).FirstOrDefault(x => x.UserId == UserId);
            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok("此帳號已刪除");
        }
        //廠商管理的所有商品資料by登入帳號
        [HttpGet("GetProduct")]
        public IActionResult GetProduct() //待測_廠商管理商品
        {
            var UserID = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var products = _context.Products.Include(p => p.ProductStatus).Include(p => p.ProductCategory)
                .Include(p => p.ProductsPictureInfos)
                .Where(p=>p.Supplier.UserId == UserID)
                .Select(p => new GetProductDto
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
        //商品新增及上傳圖片
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm] AddProductDto request)
        {
            var UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var supplier = _context.Products.Include(p=>p.Supplier).FirstOrDefault(x => x.Supplier.UserId == UserId);
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }
            if (await _context.Products.AnyAsync(s => s.ProductName == request.ProductName))
            {
                return BadRequest("商品已經存在");
            }
            var product = new Product
            {
                ProductName = request.ProductName,
                ProductCategoryId = request.ProductCategoryId,
                QuantityPerUnit = request.QuantityPerUnit,
                ProductUnitPrice = request.ProductUnitPrice,
                UnitStock = request.UnitStock,
                ProductIntroduce = request.ProductIntroduce,
                ProductStatusId = 0,
                AddedTime = DateTime.Now,
                SupplierId = supplier.SupplierId,
            };
            //為每一張上傳圖片給值及上傳位置
            if (request.ProductFiles != null)
            {
                foreach (var file in request.ProductFiles)
                {
                    var root = $@"{_env.ContentRootPath}\wwwroot\";
                    var tempRoot = "";
                    if (file.FileName.Contains(".jpg"))
                    {
                        tempRoot = root +"img"+"\\"+"Sight"+"\\" +"picture"+ "\\" + request.ProductName;
                    }
                    else
                    {
                        tempRoot = root +"img"+"\\"+"Sight"+"\\"+"other"+ "\\" +request.ProductName;
                    }
                    if (!Directory.Exists(tempRoot))
                    {
                        Directory.CreateDirectory(tempRoot);
                    }
                    var path = tempRoot +"\\" + file.FileName;

                    file.CopyTo(System.IO.File.Create(path));
                    var pic = new ProductsPictureInfo
                    {
                        ProductPicturePath = "\\" + path.Replace(root, ""),
                        ProductPictureName = request.ProductName
                    };
                    product.ProductsPictureInfos.Add(pic);
                }
            }
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok("已新增商品");
        }
        //商品修改by登入帳號
        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int? id, [FromBody] UpdateProductDto request)
        {
            var UserID = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var product = await _context.Products.Include(s=>s.Supplier)
                .FirstOrDefaultAsync(a => a.ProductId == id);
            if (product == null)
            {
                return BadRequest("無此商品");
            }
            if (product.Supplier.UserId != UserID)
            {
                return BadRequest("不是登入廠商的商品");
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }
            product.ProductName = request.ProductName;
            product.ProductStatusId = request.ProductStatusId;
            product.ProductCategoryId = request.ProductCategoryID;
            product.ProductIntroduce = request.ProductIntroduce;
            product.ProductUnitPrice = request.ProductUnitPrice;
            product.QuantityPerUnit = request.QuantityPerUnit;
            product.UnitStock = request.UnitStock;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return Ok("修改成功!");
        }
        //首頁商品主打區
        [HttpPut("GetProductHome")]
        public IActionResult GetProductHome(List<int> request)
        {
            if (request == null)
            {
                return BadRequest("無勾選設定首頁項目");
            }
            if (request.Count > 5)
            {
                return BadRequest("最大設定數量為5個");
            }
            var LastSelectedProducts = _context.Products.Where(p => p.ProductHomePage != 1);
            if (LastSelectedProducts.Any())
            {
                foreach (Product item in LastSelectedProducts)
                {
                    item.ProductHomePage = 1;
                }
            }


            var SelectedProducts = _context.Products.Where(p => request.Contains(p.ProductId));
            foreach (Product item in SelectedProducts)
            {
                item.ProductHomePage = 0;
            }

            _context.SaveChanges();
            return Ok("已設定");
        }
    }
}
