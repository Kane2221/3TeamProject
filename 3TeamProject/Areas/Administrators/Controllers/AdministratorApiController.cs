using _3TeamProject.Areas.Administrators.Data;
using _3TeamProject.Areas.Members.Data;
using _3TeamProject.Areas.SocialActivities.Data;
using _3TeamProject.Areas.Suppliers.Data;
using _3TeamProject.Models;
using _3TeamProject.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Security.Cryptography;

namespace _3TeamProject.Areas.Administrators.Controllers
{

    [Authorize(Roles = ("Administrator, ChiefAdministrator, SuperAdministrator"))]
    [Route("Administrators/[controller]")]
    [ApiController]
    public class AdministratorApiController : ControllerBase
    {
        private readonly _3TeamProjectContext _context;

        private readonly IConfiguration _config;

        private readonly MailService _mailService;

        public IHostEnvironment _env { get; }
        public int ActivityMessageCount { get; private set; }

        public AdministratorApiController(_3TeamProjectContext Context, IConfiguration config, IHostEnvironment env, MailService mailService)
        {
            _context = Context;
            _config = config;
            _env = env;
            _mailService = mailService;
        }
        [HttpGet("GetAllAdmins")]//權限Administrator只能看見同權限以下的清單，更高權限可以看見所有人清單
        public ActionResult<IEnumerable<GetAdminDto>> GetAllAdmins()
        {
            var admin = _context.Administrators.Include(u => u.User)
                       .Select(u => new GetAdminDto
                       {
                           UserId = u.UserId,
                           Account = u.User.Account,
                           Email = u.User.Email,
                           Roles = u.User.RolesNavigation.RoleName,
                           AdministratorName = u.AdministratorName,
                           PhoneNumber = u.PhoneNumber
                       });
            string UserRole = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            if (UserRole == "Administrator")
            {
                admin = admin.Where(u => u.Roles == "5");
            }
            else if (UserRole == "ChiefAdministrator")
            {
                admin = admin.Where(u => u.Roles != "3");
            }

            return Ok(admin);
        }
        [HttpGet("GetAdmin")]//登入管理員的資料
        public ActionResult<GetAdminDto> GetAdmins()
        {
            var UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var admin = _context.Administrators.Include(u => u.User).Where(u => u.UserId == UserId).Select(u => new GetAdminDto
            {
                UserId = u.UserId,
                Account = u.User.Account,
                Email = u.User.Email,
                Roles = u.User.RolesNavigation.RoleName,
                AdministratorName = u.AdministratorName,
                PhoneNumber = u.PhoneNumber
            }).SingleOrDefault();
            if (admin == null)
            {
                return NotFound("帳號不存在");
            }
            return Ok(admin);
        }
        //新增管理員, 最高權限才能新增。
        [Authorize(Roles = "SuperAdministrator")]
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
                    AdministratorStatusId = 1,
                    User = new User
                    {
                        Account = request.Account,
                        Email = request.Email,
                        PasswordHash = passwordHsah,
                        PasswordSalt = passwordSalt,
                        VerificationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
                        Roles = request.Roles
                    }
                };
                _context.Administrators.Add(admin);
                await _context.SaveChangesAsync();
                User? user = _context.Users.Where(u => u.Account == request.Account).FirstOrDefault();
                _mailService.SendMail(request.Email, "新增員工帳號", "<h1>管理員帳號 : {request.Account}, 管理員密碼 : {request.Password}</h1>/n");
                return Ok($"已新增員工帳號:{user?.UserId}");
            }
        }
        //修改管理員資料
        [HttpPut("UpdateAdmin/{id}")]
        public async Task<IActionResult> UpdateAdmin(int? id, [FromBody] UpdateAdminDto request)
        {
            var UserRole = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            var UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var admin = await _context.Administrators.Include(a => a.User)
                    .Where(a => a.UserId == id).Select(a => a).FirstOrDefaultAsync();
            //如果id不一樣或著不是主管以上管理員，無權限修改此id資料。
            if (UserId != id && UserRole == "Administrator")
            {
                return BadRequest("與登入帳號不符或無權限修改");
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
            if (request.Password != "********")
            {
                using (var hmac = new HMACSHA512())
                {
                    var passwordSalt = hmac.Key;
                    var passwordHsah = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));
                    admin.User.PasswordSalt = passwordSalt;
                    admin.User.PasswordHash = passwordHsah;
                }
            }

            admin.User.Email = request.Email;
            admin.AdministratorName = request.AdministratorName;
            admin.PhoneNumber = request.PhoneNumber;
            admin.User.Roles = request.Roles;
            _context.Administrators.Update(admin);
            await _context.SaveChangesAsync();
            return Ok("修改成功!");
        }
        //刪除管理員
        [HttpDelete("DeleteAdmin/{id}")]
        [Authorize(Roles = "SuperAdministrator")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            User? admin = _context.Users.Include(u => u.Administrators).FirstOrDefault(x => x.UserId == id);
            if (admin == null)
            {
                return BadRequest("無此帳號");
            }
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
                               SupplierStatus = s.SupplierStatus.StatusName,
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
                AddedTime = p.AddedTime.Value.ToShortDateString(),
                RemovedTime = p.RemovedTime.Value.ToShortDateString(),
                ProductIntroduce = p.ProductIntroduce,
                StatusName = p.ProductStatus.StatusName,
                ProductHomePage = p.ProductHomePage,
                ProductStatusId = p.ProductStatusId,
            });
            return Ok(products);
        }
        //取得所有訂單
        [HttpGet("GetAllOrders")]
        public IActionResult GetAllOrders()
        {
            var orderlist = _context.Orders.Include(o => o.Member).Include(o => o.OrderDetails).ThenInclude(od => od.Product).Include(o => o.OrderStatusNavigation)
                                .Include(o => o.PaymentStatusNavigation).Include(o => o.ShipStatusNavigation).Select(o => new GetAllOrdersDto
                                {
                                    OrderId = o.OrderId,
                                    MemberId = o.MemberId,
                                    AdministratorId = o.AdministratorId,
                                    OrderDate = o.OrderDate.ToShortDateString(),
                                    ShipDate = o.ShipDate.ToShortDateString(),
                                    OrderCategoryName = o.OrderStatusNavigation.OrderCategoryName,
                                    PaymentCategoryName = o.PaymentStatusNavigation.PaymentCategoryName,
                                    ShipCategoryName = o.ShipStatusNavigation.ShipCategoryName,
                                    ShipPostalCode = o.ShipPostalCode,
                                    ShipCountry = o.ShipCountry,
                                    ShipCity = o.ShipCity,
                                    ShipAddress = o.ShipAddress,
                                    Total = o.OrderDetails.Sum(odd => odd.UnitPrice * odd.Quantity * (1 - (decimal)odd.Discount)),
                                    OrderDetails = o.OrderDetails.Select(od => new GetOrderDetailDto
                                    {
                                        ProductId = od.ProductId,
                                        ProductName = od.Product.ProductName,
                                        UnitPrice = od.UnitPrice,
                                        Discount = od.Discount,
                                        Quantity = od.Quantity,
                                        SubTotal = od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)
                                    })
                                });
            return Ok(orderlist);
        }
        //取得所有景點清單
        [HttpGet("GetAllSightseeings")]
        public IActionResult GetAllSightseeings()
        {
            var Sight = _context.Sightseeings.Include(s => s.SightseeingPictureInfos)
                .Include(s => s.SightseeingCategory).Select(s => new GetSightByAdminDto
                {
                    SightseeingId = s.SightseeingId,
                    SightseeingName = s.SightseeingName,
                    SightseeingCountry = s.SightseeingCountry,
                    SightseeingCity = s.SightseeingCity,
                    SightseeingAddress = s.SightseeingAddress,
                    SightseeingScore = s.SightseeingScore,
                    CategoryName = s.SightseeingCategory.CategoryName,
                    SightseeingCategoryId = s.SightseeingCategoryId,
                    SightseeingIntroduce = s.SightseeingIntroduce,
                    SightseeingHomePage = s.SightseeingHomePage,
                    SightseeingPictureInfos = s.SightseeingPictureInfos.Select(p => new GetSightPicInfoByAdminDto
                    {
                        SightseeingPictureId = p.SightseeingPictureId,
                        SightseeingPictureName = p.SightseeingPictureName,
                        SightseeingPicturePath = p.SightseeingPicturePath
                    })
                });

            return Ok(Sight);
        }
        //社群活動管理頁
        [HttpGet("GetAllActivities")]
        public IActionResult GetAllActivities()
        {
            var Activities = _context.SocialActivities.Include(s => s.Member).Select(s => new GetAllActivitiesDto
            {
                ActivityId = s.ActivityId,
                MemberName = s.Member.MemberName,
                ActivitiesName = s.ActivitiesName,
                ActivitiesAddress = s.ActivitiesAddress,
                CreatedTime = s.CreatedTime.ToShortDateString(),
                EndTime = s.EndTime.ToShortDateString(),
                LimitCount = s.LimitCount,
                JoinCount = s.JoinCount
            });
            return Ok(Activities);
        }
        //審核廠商資料，並寄信通知。
        [HttpPut("AproveSupplier/{id}")]
        public IActionResult AproveSupplier(int id, SupplierStatusDto request)
        {
            var supplier = _context.Suppliers.Include(s => s.User).Where(s => s.UserId == id).FirstOrDefault();
            if (supplier == null)
            {
                return NotFound("找不到此帳號");
            }
            supplier.SupplierStatusId = 1;
            supplier.User.VerfiedAt = DateTime.Now;
            _context.SaveChanges();
            _mailService.SendMail(supplier.User.Email, "帳號已審核", "<h1>您的帳號已通過審核!</h1>");

            return Ok("廠商申請帳號已審核通過");
        }
        //廠商權限修改(停權/回復權限)
        [HttpPut("SuspendSupplier/{id}")]
        public IActionResult SuspendSupplier(int id, [FromBody] SupplierStatusDto request)
        {
            var supplier = _context.Suppliers.Include(s => s.User).Where(s => s.UserId == id).FirstOrDefault();

            if (supplier == null)
            {
                return NotFound("找不到此帳號");
            }
            if (request.SupplierStatus == "使用中")
            {
                supplier.SupplierStatusId = 2;
                _mailService.SendMail(supplier.User.Email, "帳號停權", "<h1>您的帳號已被停權，請聯絡管理人員!</h1>");
                _context.SaveChanges();
                return Ok("廠商已停權");
            }
            else if (request.SupplierStatus == "停權")
            {
                supplier.SupplierStatusId = 1;
                _mailService.SendMail(supplier.User.Email, "帳號回復權限", "<h1>您的帳號已回復權限!</h1>");
                _context.SaveChanges();
                return Ok("廠商已回復權限");
            }
            return BadRequest("無此帳號");

        }
        //會員權限修改(停權/回復權限)
        [HttpPut("SuspendMember/{id}")]
        public IActionResult SuspendMember(int id, [FromBody] MemberStatusDTO request)
        {
            var member = _context.Members.Include(s => s.User).Where(s => s.UserId == id).FirstOrDefault();

            if (member == null)
            {
                return NotFound("找不到此帳號");
            }
            if (request.MemberStatus == "使用中")
            {
                member.MemberStatusId = 3;
                _mailService.SendMail(member.User.Email, "帳號停權", "<h1>您的帳號已被停權，請聯絡管理人員!</h1>");
                _context.SaveChanges();
                return Ok("會員已停權");
            }
            else if (request.MemberStatus == "停權")
            {
                member.MemberStatusId = 2;
                _mailService.SendMail(member.User.Email, "帳號回復權限", "<h1>您的帳號已回復權限!</h1>");
                _context.SaveChanges();
                return Ok("會員已回復權限");
            }
            return BadRequest("無此帳號");
        }
        //景點新增及上傳圖片
        [HttpPost("AddSight")]
        public async Task<IActionResult> AddSight([FromForm] AddSightDto request)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var admin = _context.Sightseeings.FirstOrDefault(x => x.Administrator.UserId == UserId);
            if (admin == null)
            {
                return BadRequest("無此管理員");
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }
            if (await _context.Sightseeings.AnyAsync(s => s.SightseeingName == request.SightseeingName))
            {
                return BadRequest("景點已經存在");
            }
            Sightseeing sight = new Sightseeing
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
                        tempRoot = root + "img" + "\\" + "Sight" + "\\" + "picture" + "\\" + request.SightseeingName;
                    }
                    else
                    {
                        tempRoot = root + "img" + "\\" + "Sight" + "\\" + "other" + "\\" + request.SightseeingName;
                    }
                    if (!Directory.Exists(tempRoot))
                    {
                        Directory.CreateDirectory(tempRoot);
                    }
                    var path = tempRoot + "\\" + file.FileName;

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
        //修改景點資訊
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
            var Sight = _context.Sightseeings.Where(s => s.SightseeingId == id)
                .Select(s => s).SingleOrDefault();
            if (Sight != null)
            {
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
            return NotFound("找不到此景點");

        }
        //景點刪除
        [HttpDelete("DeleteSight/{id}")]
        public async Task<IActionResult> DeleteSight(int id)
        {
            var sightseeings = _context.Sightseeings.Include(s => s.SightseeingMessageBoards).Include(s => s.SightseeingPictureInfos)
                .FirstOrDefault(x => x.SightseeingId == id);
            if (sightseeings == null)
            {
                return BadRequest("沒有此景點");
            }
            _context.Sightseeings.Remove(sightseeings);
            await _context.SaveChangesAsync();
            return Ok("此景點已刪除");
        }
        //修改會員密碼並送信給會員(最高權限管理員)
        [Authorize(Roles = "SuperAdministrator")]
        [HttpPut("UpdateMember/{id}")]
        public async Task<IActionResult> UpdateMemberPassword(int id, [FromBody] UpdateMemberPasswordDto request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }
            var member = _context.Members.Include(a => a.User)
                    .Where(a => a.UserId == id).SingleOrDefault();
            if (member == null)
            {
                return NotFound("找不到此帳號");
            }

            using (var hmac = new HMACSHA512())
            {
                var passwordSalt = hmac.Key;
                var passwordHsah = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));
                member.User.PasswordHash = passwordHsah;
                member.User.PasswordSalt = passwordSalt;
                member.User.VerfiedAt = DateTime.UtcNow;
                _context.Members.Update(member);
                _mailService.SendMail(member.User.Email, "新密碼", "$\"<h1>您的新密碼為:{request.Password}，請去個人資料修改密碼</h1>");
                await _context.SaveChangesAsync();
            }
            return Ok("修改成功!");

        }
        //TODO 後台首頁
        [HttpGet("GetDash")]
        public IActionResult GetDash()
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            
            var Dash = new GetDashDto()
            {

                ActivityCount = _context.SocialActivities.Count(),
                ActivityMessageCount = _context.ActivitiesMessageBoards.Count(),
                MemberCount = _context.Members.Count(),
                UnpaymentOrderCount = _context.Orders.Count(o => o.PaymentStatus == 0),
                UnproveProductCount = _context.Products.Count(p => p.ProductStatusId != 0),
                UnproveSpplierCount = _context.Suppliers.Count(s => s.SupplierStatusId != 0),
                UnShipOrderCount = _context.Orders.Where(o => o.PaymentStatus == 1 && o.ShipStatus != 0).Count(),
                MonthlySales = _context.OrderDetails.Include(od => od.Order)
                .Where(od => od.Order.ShipDate >= startOfMonth && od.Order.ShipDate <= endOfMonth)
                .Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)),
                SixMonthlySales = Enumerable.Range(0, 6)
                    .Select(i => startOfMonth.AddMonths(-i))
                    .Select(date => new GetSixMonthlySales
                    {
                        Date = date.Year.ToString() + "/" +date.Month.ToString(),
                        Sales = _context.OrderDetails
                            .Include(od => od.Order)
                            .Where(od => od.Order.ShipDate.Year == date.Year && od.Order.ShipDate.Month == date.Month)
                            .Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount))
                    })
                    .ToList()
                };
            return Ok(Dash);
        }
        //取得所有會員資料
        [HttpGet("GetAlltMember")]
        public IActionResult GetAlltMember()
        {
            var member = (from u in _context.Users
                          join m in _context.Members
                          on u.UserId equals m.UserId
                          join s in _context.MemberStatusCategories
                          on m.MemberStatusId equals s.MemberStatusId
                          select new GetMemberDto
                          {
                              UserId = u.UserId,
                              Account = u.Account,
                              Email = u.Email,
                              RoleName = u.RolesNavigation.RoleName,
                              MemberName = m.MemberName,
                              NickName = m.NickName,
                              Birthday = m.Birthday.ToShortDateString(),
                              IdentityNumber = m.IdentityNumber,
                              CellPhoneNumber = m.CellPhoneNumber,
                              PhoneNumber = m.PhoneNumber,
                              PostalCode = m.PostalCode,
                              Country = m.Country,
                              City = m.City,
                              Address = m.Address,
                              Age = m.Age,
                              PicturePath = u.PicturePath,
                              MemberStatus = s.StatusName
                          });
            return Ok(member);
        }
        //廠商修改密碼並寄信通知。(最高權限管理員)
        [Authorize(Roles = "SuperAdministrator")]
        [HttpPut("UpdateSupplier/{id}")]
        public async Task<IActionResult> UpdateSupplierPassword(int id, [FromBody] UpdateSupplierPasswordDto request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }
            var supplier = _context.Suppliers.Include(a => a.User)
                    .Where(a => a.UserId == id).SingleOrDefault();
            if (supplier == null)
            {
                return NotFound("找不到此帳號");
            }

            using (var hmac = new HMACSHA512())
            {
                var passwordSalt = hmac.Key;
                var passwordHsah = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));
                supplier.User.PasswordHash = passwordHsah;
                supplier.User.PasswordSalt = passwordSalt;
                supplier.User.VerfiedAt = DateTime.UtcNow;
                _context.Suppliers.Update(supplier);
                _mailService.SendMail(supplier.User.Email, "新密碼", "$\"<h1>您的新密碼為:{request.Password}，請去個人資料修改密碼</h1>");
                await _context.SaveChangesAsync();
            }
            return Ok("修改成功!");

        }
        //會員刪除資料(最高權限管理員)
        [Authorize(Roles = "SuperAdministrator")]
        [HttpDelete("DeleteMember/{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            Member? member = _context.Members.Include(u => u.User).FirstOrDefault(x => x.UserId == id);
            if (member == null)
            {
                return BadRequest("無此帳號");
            }
            member.MemberStatusId = 4;
            _context.Members.Update(member);
            await _context.SaveChangesAsync();
            return Ok("此帳號已刪除");
        }
        //廠商刪除資料(最高權限管理員)
        [Authorize(Roles = "SuperAdministrator")]
        [HttpDelete("DeleteSupplier/{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            Supplier? supplier = _context.Suppliers.Include(u => u.User).FirstOrDefault(x => x.UserId == id);
            if (supplier == null)
            {
                return BadRequest("無此帳號");
            }
            supplier.SupplierStatusId = 3;
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
            return Ok("此帳號已刪除");
        }
        //商品修改資料
        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int? id, [FromBody] UpdateProductDto request)
        {
            var product = await _context.Products.FirstOrDefaultAsync(a => a.ProductId == id);
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }
            if (product == null)
            {
                return BadRequest("無此商品");
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
        //設定商品主打區
        [HttpPut("UpdateProductHome")]
        public IActionResult UpdateProductHome(List<int> request)
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
        //設定景點主打區
        [HttpPut("UpdateSightHome")]
        public IActionResult UpdateSightHome(List<int> request)
        {
            if (request == null)
            {
                return BadRequest("無勾選設定首頁項目");
            }
            if (request.Count > 3)
            {
                return BadRequest("最大設定數量為3個");
            }
            var LastSelectedSightseeings = _context.Sightseeings.Where(s => s.SightseeingHomePage != 1);
            if (LastSelectedSightseeings.Any())
            {
                foreach (Sightseeing item in LastSelectedSightseeings)
                {
                    item.SightseeingHomePage = 1;
                }
            }


            var SelectedSightseeings = _context.Sightseeings.Where(s => request.Contains(s.SightseeingId));
            foreach (Sightseeing item in SelectedSightseeings)
            {
                item.SightseeingHomePage = 0;
            }

            _context.SaveChanges();
            return Ok("已設定");
        }
    }
}

