using _3TeamProject.Areas.Administrators.Data;
using _3TeamProject.Areas.Members.Data;
using _3TeamProject.Areas.Suppliers.Data;
using _3TeamProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Security.Cryptography;
using SightGetViewModel = _3TeamProject.Areas.Administrators.Data.GetSightDto;

namespace _3TeamProject.Areas.Administrators.Controllers
{

    [Authorize(Roles = ("Administrator, ChiefAdministrator, SuperAdministrator"))]
    [Route("Administrators/[controller]")]
    [ApiController]
    public class AdministratorApiController : ControllerBase
    {
        private readonly _3TeamProjectContext _context;

        private readonly IConfiguration _config;

        public IHostEnvironment _env { get; }

        public AdministratorApiController(_3TeamProjectContext Context, IConfiguration config, IHostEnvironment env)
        {
            _context = Context;
            _config = config;
            _env=env;
        }
        [HttpGet("GetAllAdmins")]//權限Administrator只能看見同權限以下的清單，更高權限可以看見所有人清單
        public IActionResult GetAllAdmins()
        {
            var UserRole = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            if (UserRole == "Administrator")
            {
                var admin = _context.Administrators.Include(u => u.User)
                        .Where(u => u.User.Roles == 5).Select(u => new GetAdminDto
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
                    .Where(u => u.User.Roles != 3).Select(u => new GetAdminDto
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
                    .Select(u => new GetAdminDto
                    {
                        Account = u.User.Account,
                        Email = u.User.Email,
                        Roles = u.User.RolesNavigation.RoleName,
                        AdministratorName = u.AdministratorName,
                        PhoneNumber = u.PhoneNumber
                    });
            return Ok(adminSuper);
        }
        //新增管理員, 最高權限才能新增。
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] AddNewAdminDto request)
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
                    AdministratorStatusId = 0,
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
        //TODO 新增審核管理員註冊
        //修改管理員資料
        [HttpPut("UpdateAdmin/{id}")]
        public async Task<IActionResult> UpdateAdmin(int? id, [FromBody] UpdateAdminDto request)
        {
            var UserRole = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            var UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var admin = await _context.Administrators.Include(a => a.User)
                    .Where(a => a.UserId == id).Select(a => a).FirstOrDefaultAsync();
            //如果id不一樣或著不是主管以上管理員，無權限修改此id資料。
            if (UserId != id && UserRole != "SuperAdministrator" && UserRole != "ChiefAdministrator")
            {
                return BadRequest("與登入帳號不符");
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }
            if (admin == null)
            {
                return BadRequest("無此帳號");
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
        //刪除管理員
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdministrator")]
        public async Task<IActionResult> Delete(int id)
        {
            User admin = _context.Users.Include(u => u.Administrators).FirstOrDefault(x => x.UserId == id);
            _context.Users.Remove(admin);
            await _context.SaveChangesAsync();
            return Ok("此帳號已刪除");
        }
        //取得所有廠商清單
        [HttpGet("GetAllSuppliers")]
        public IActionResult GetAllSuppliers()
        {
            var supplier = from u in _context.Users
                           join s in _context.Suppliers
                           on u.UserId equals s.UserId
                           select new GetSupplierDto
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
        //取得所有商品
        [HttpGet("GetAllProduct")]
        public IActionResult GetAllProducts()
        {
            var products = _context.Products.Include(p => p.ProductStatus).Include(p => p.ProductCategory).Select(p => new GetAllProductDto
            {
                ProductId = p.ProductId,
                CategoryName = p.ProductCategory.CategoryName,
                ProductName = p.ProductName,
                QuantityPerUnit = p.QuantityPerUnit,
                ProductUnitPrice = p.ProductUnitPrice,
                UnitStock = p.UnitStock,
                UniOnOrder = p.UniOnOrder,
                ProductRecommendation = p.ProductRecommendation,
                AddedTime = p.AddedTime,
                RemovedTime = p.RemovedTime,
                ProductIntroduce = p.ProductIntroduce,
                StatusName = p.ProductStatus.StatusName,
                ProductHomePage = p.ProductHomePage
            });
            return Ok(products);
        }
        [HttpGet("GetAllOrders")]
        public IActionResult GetAllOrders() // TODO 待測_訂單管理頁 
        {
            var orderlist = _context.Orders.Include(o => o.Member).Include(o => o.OrderDetails).Include(o => o.OrderStatusNavigation)
                                .Include(o => o.PaymentStatusNavigation).Include(o => o.ShipStatusNavigation).Select(o => new GetAllOrdersDto
                                {
                                    OrderId = o.OrderId,
                                    MemberId = o.MemberId,
                                    AdministratorId = o.AdministratorId,
                                    OrderDate = o.OrderDate,
                                    ShipDate = o.ShipDate,
                                    OrderCategoryName = o.OrderStatusNavigation.OrderCategoryName,
                                    PaymentCategoryName = o.PaymentStatusNavigation.PaymentCategoryName,
                                    ShipCategoryName = o.ShipStatusNavigation.ShipCategoryName,
                                    ShipPostalCode = o.ShipPostalCode,
                                    ShipCountry = o.ShipCountry,
                                    ShipCity = o.ShipCity,
                                    ShipAddress = o.ShipAddress,
                                    OrderDetails = o.OrderDetails.Select(od => new OrderDetailDto
                                    {
                                        ProductId = od.ProductId,
                                        UnitPrice = od.UnitPrice,
                                        Discount = od.Discount,
                                        Quantity = od.Quantity,
                                    })
                                });
            return Ok(orderlist);
        }
        //取得所有景點清單
        [HttpGet("GetAllSightseeing")]
        public IActionResult GetAllSightseeings()
        {
            var Sight = _context.Sightseeings.Include(s => s.SightseeingPictureInfos)
                .Include(s => s.SightseeingCategory).Select(s => new SightGetViewModel
                {
                    SightseeingId = s.SightseeingId,
                    SightseeingName = s.SightseeingName,
                    SightseeingCountry = s.SightseeingCountry,
                    SightseeingCity = s.SightseeingCity,
                    SightseeingAddress = s.SightseeingAddress,
                    SightseeingScore = s.SightseeingScore,
                    CategoryName = s.SightseeingCategory.CategoryName,
                    SightseeingHomePage = s.SightseeingHomePage,
                });
            return Ok(Sight);
        }

        [HttpGet("GetAllActivities")]
        public IActionResult GetAllActivities() // TODO 社群活動管理頁
        {
            return Ok();
        }
        //TODO 審核廠商
        //TODO 廠商停權
        //TODO 會員停權
        //TODO 商品上下架
        //TODO 審核商品
        //TODO 審核訂單退訂
        //景點新增及上傳圖片
        [HttpPost("AddSight")]
        public async Task<IActionResult> AddSight([FromForm] AddSightDto request)
        {
            var UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var admin = _context.Sightseeings.FirstOrDefault(x => x.Administrator.UserId == UserId);
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }
            if (await _context.Sightseeings.AnyAsync(s => s.SightseeingName == request.SightseeingName))
            {
                return BadRequest("景點已經存在");
            }
            var sight = new Sightseeing
            {
                SightseeingName = request.SightseeingName,
                SightseeingCountry = request.SightseeingCountry,
                SightseeingCity = request.SightseeingCity,
                SightseeingAddress = request.SightseeingAddress,
                SightseeingScore = request.SightseeingScore,
                AdministratorId = admin.AdministratorId,
                SightseeingIntroduce = request.SightseeingIntroduce,
                SightseeingCategoryId = request.SightseeingCategoryId,
            };
            //為每一張上傳圖片給值及上傳位置
            if (request.Files != null)
            {
                foreach (var file in request.Files)
                {
                    var root = $@"{_env.ContentRootPath}\wwwroot\";
                    var tempRoot = "";
                    if (file.FileName.Contains(".jpg"))
                    {
                        tempRoot = root +"img"+"\\"+"Sight"+"\\" +"picture"+ "\\" + request.SightseeingName;
                    }
                    else
                    {
                        tempRoot = root +"img"+"\\"+"Sight"+"\\"+"other"+ "\\" +request.SightseeingName;
                    }
                    if (!Directory.Exists(tempRoot)) 
                    {
                        Directory.CreateDirectory(tempRoot);
                    }
                    var path = tempRoot +"\\" + file.FileName;

                    file.CopyTo(System.IO.File.Create(path));
                    var pic = new SightseeingPictureInfo
                    {
                        SightseeingPicturePath = "\\" + path.Replace(root, ""),
                        SightseeingPictureName = request.SightseeingName
                    };
                    sight.SightseeingPictureInfos.Add(pic);
                }
            }
            _context.Sightseeings.Add(sight);
            _context.SaveChanges();
            return Ok("已新增景點");
        }
        //TODO 修改景點訊息OK, 缺圖片置換或新增功能
        [HttpPut("UpdateSight/{id}")]
        public IActionResult UpdateSight(int id, UpdateSightDto request)
        {
            var sid = _context.Sightseeings.FirstOrDefault(s => s.SightseeingId == id);
            if (sid == null)
            {
                return BadRequest("沒有此景點");
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }
            var Sight = _context.Sightseeings.Where(s => s.SightseeingId == id).Select(s => s).SingleOrDefault();
            Sight.SightseeingName = request.SightseeingName;
            Sight.SightseeingCountry = request.SightseeingCountry;
            Sight.SightseeingCity = request.SightseeingCity;
            Sight.SightseeingAddress = request.SightseeingAddress;
            Sight.SightseeingScore = request.SightseeingScore;
            Sight.SightseeingIntroduce = request.SightseeingIntroduce;
            Sight.SightseeingHomePage = request.SightseeingHomePage;
            Sight.SightseeingCategoryId = request.SightseeingCategoryId;
            _context.Sightseeings.Update(Sight);
            _context.SaveChanges();
            return Ok("已修改");
        }
        //景點刪除
        [HttpDelete("DeleteSight/{id}")]
        public async Task<IActionResult> DeleteSight(int id)
        {
            var sid = _context.Sightseeings.FirstOrDefault(s => s.SightseeingId == id);
            if (sid == null)
            {
                return BadRequest("沒有此景點");
            }
            var sightseeings = _context.Sightseeings.Include(s => s.SightseeingMessageBoards).Include(s => s.SightseeingPictureInfos)
                .Where(x => x.SightseeingId == id).FirstOrDefault();
            _context.Sightseeings.Remove(sightseeings);
            await _context.SaveChangesAsync();
            return Ok("此景點已刪除");
        }
        //TODO 修改會員資料(最高權限管理員)
        [Authorize(Roles ="SuperAdministrator")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMemberDto request)
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

            var member = _context.Members.Include(a => a.User)
                    .Where(a => a.UserId == id).Select(a => a).SingleOrDefault();

            using (var hmac = new HMACSHA512())
            {
                var passwordSalt = hmac.Key;
                var passwordHsah = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));
                member.MemberName = request.MemberName;
                member.NickName = request.NickName;
                member.Birthday = request.Birthday;
                member.IdentityNumber = request.IdentityNumber;
                member.CellPhoneNumber = request.CellPhoneNumber;
                member.PhoneNumber = request.PhoneNumber;
                member.PostalCode = request.PostalCode;
                member.Country = request.Country;
                member.City = request.City;
                member.Address = request.Address;
                member.User.Email = request.Email;
                member.User.PasswordHash = passwordHsah;
                member.User.PasswordSalt = passwordSalt;
                _context.Members.Update(member);
                await _context.SaveChangesAsync();
            }
            return Ok("修改成功!");
        }
        //TODO 後台首頁功能
    }
}
    
