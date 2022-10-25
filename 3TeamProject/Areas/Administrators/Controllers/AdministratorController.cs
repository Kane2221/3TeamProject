using _3TeamProject.Areas.Administrators.Data;
using _3TeamProject.Areas.Sightseeings.Data;
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
using SightGetViewModel = _3TeamProject.Areas.Administrators.Data.SightGetViewModel;

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
        //新增管理員, 最高權限才能新增。
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] AddNewAdminViewModel request)
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
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int? id, [FromBody] AdminUpdateViewModel request)
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
            User user = _context.Users.Include(u => u.Administrators).FirstOrDefault(x => x.UserId == id);
            _context.Users.Remove(user);
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
        //取得所有商品
        [HttpGet("GetAllProduct")]
        public IActionResult GetAllProducts()
        {
            var products = _context.Products.Include(p => p.ProductStatus).Include(p => p.ProductCategory).Select(p => new GetAllProductViewModel
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
                                .Include(o => o.PaymentStatusNavigation).Include(o => o.ShipStatusNavigation).Select(o => new GetAllOrdersViewModel
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
                                    OrderDetails = o.OrderDetails.Select(od => new OrderDetailViewModel
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
        //TODO 修改景點訊息
        //TODO 景點上傳
        //TODO 景點刪除
        //TODO 修改會員資料(最高權限管理員)
        //TODO 後台首頁功能
    }
}
