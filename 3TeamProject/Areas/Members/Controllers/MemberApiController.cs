using _3TeamProject.Areas.Administrators.Data;
using _3TeamProject.Areas.Members.Data;
using _3TeamProject.Data;
using _3TeamProject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;

namespace _3TeamProject.Areas.Members.Controllers
{
    [Authorize(Roles = "Members")]
    [Route("Members/[controller]")]
    [ApiController]
    public class MemberApiController : ControllerBase
    {

        private readonly _3TeamProjectContext _context;
        private readonly IConfiguration _config;

        public MemberApiController(_3TeamProjectContext Context, IConfiguration config)
        {
            _context = Context;
            _config = config;
        }
        //取得會員資料
        [HttpGet("GetMember")]
        public IActionResult GetMember()
        {
            var UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var user = _context.Users.Include(u => u.Members).FirstOrDefault(x => x.UserId == UserId);
            var member = (from u in _context.Users
                          where u.UserId == user.UserId
                          join m in _context.Members
                          on u.UserId equals m.UserId
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
                              PicturePath = u.PicturePath
                          }).SingleOrDefault();
            return Ok(member);
        }
        //會員註冊
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterMemberDto request)
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
                Member member = new Member
                {
                    MemberName = request.MemberName,
                    NickName = request.NickName,
                    Birthday = request.Birthday,
                    IdentityNumber = request.IdentityNumber,
                    CellPhoneNumber = request.CellPhoneNumber,
                    PhoneNumber = request.PhoneNumber,
                    PostalCode = request.PostalCode,
                    Country = request.Country,
                    City = request.City,
                    Address = request.Address,
                    MemberStatusId = 1,
                    User = new User
                    {
                        Account = request.Account,
                        Email = request.Email,
                        PasswordHash = passwordHsah,
                        PasswordSalt = passwordSalt,
                        VerificationToken = verifyToken,
                        Roles = 1
                    }
                };
                #region Send Email with verify code (正式再解開註解)
                //var root = $@"{Request.Scheme}:/{Request.Host}/Member/Verify?{verifyToken}";
                var root = $@"https://localhost:7007/Member/Verify";
                //TODO 修改寄信的超連結
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("dotnettgm102@gmail.com", "帳號驗證碼");
                    mail.To.Add(request.Email);
                    mail.Priority = MailPriority.Normal;
                    mail.Subject = "帳號驗證碼";
                    mail.Body = $"<h1>請點選以下連結驗證您的帳號</h1> " +
                                $"<a href=\"https://localhost:7007/Member/Verify?token={verifyToken}&account={request.Account}\"><h1>帳號驗證碼</h1></a>";
                    mail.IsBodyHtml = true;
                    SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);
                    MySmtp.UseDefaultCredentials = false;
                    MySmtp.Credentials = new System.Net.NetworkCredential(_config["mail:Account"], _config["mail:Password"]);
                    MySmtp.EnableSsl = true;
                    MySmtp.Send(mail);
                    MySmtp = null;
                };
                #endregion

                _context.Members.Add(member);
                await _context.SaveChangesAsync();
                return Ok("註冊成功，請等待驗證信件");
            }
        }
        //會員資料修改
        [HttpPut("Update/{id}")]
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
        //會員刪除
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            if (id != UserId)
            {
                return BadRequest("與登入帳號不符");
            }
            var member = _context.Members.Include(u => u.User).FirstOrDefault(x => x.UserId == id);
            member.MemberStatusId = 4;
            _context.Members.Update(member);
            await _context.SaveChangesAsync();
            return Ok("此帳號已刪除");
        }
        //會員我的訂單
        [HttpGet("GetOrder")]
        public IActionResult GetOrder()
        {
            var UserID = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var myOrder = _context.Orders.Include(o => o.Member).Include(o => o.OrderDetails)
                .Include(o => o.OrderStatusNavigation).Include(o => o.PaymentStatusNavigation)
                .Include(o => o.ShipStatusNavigation).Where(o => o.Member.UserId == UserID && o.ShipStatus != 2)
                .Select(o => new GetAllOrdersDto
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate.ToShortDateString(),
                    ShipDate = o.ShipDate.ToShortDateString(),
                    OrderCategoryName = o.OrderStatusNavigation.OrderCategoryName,
                    PaymentCategoryName = o.PaymentStatusNavigation.PaymentCategoryName,
                    ShipCategoryName = o.ShipStatusNavigation.ShipCategoryName,
                    Total = o.OrderDetails.Sum(odd => odd.UnitPrice*odd.Quantity*(1-(decimal)odd.Discount)),
                    OrderDetails = o.OrderDetails.Select(od => new GetOrderDetailDto
                    {
                        ProductId = od.ProductId,
                        ProductName = od.Product.ProductName,
                        UnitPrice = od.UnitPrice,
                        Discount = od.Discount,
                        Quantity = od.Quantity,
                        SubTotal = od.UnitPrice*od.Quantity*(1-(decimal)od.Discount)
                    })
                });
            return Ok(myOrder);
        }
        //會員訂購記錄
        [HttpGet("GetOrderRecord")]
        public IActionResult GetOrderRecord()
        {
            var UserID = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var OrderRecord = _context.Orders.Include(o => o.Member).Include(o => o.OrderDetails)
                .Include(o => o.OrderStatusNavigation).Include(o => o.PaymentStatusNavigation)
                .Include(o => o.ShipStatusNavigation).Where(o => o.Member.UserId == UserID && o.ShipStatus == 2)
                                .Select(o => new GetAllOrdersDto
                                {
                                    OrderId = o.OrderId,
                                    OrderDate = o.OrderDate.ToShortDateString(),
                                    ShipDate = o.ShipDate.ToShortDateString(),
                                    OrderCategoryName = o.OrderStatusNavigation.OrderCategoryName,
                                    PaymentCategoryName = o.PaymentStatusNavigation.PaymentCategoryName,
                                    ShipCategoryName = o.ShipStatusNavigation.ShipCategoryName,
                                    Total = o.OrderDetails.Sum(odd=> odd.UnitPrice*odd.Quantity*(1-(decimal)odd.Discount)),
                                    OrderDetails = o.OrderDetails.Select(od => new GetOrderDetailDto
                                    {
                                        ProductId = od.ProductId,
                                        UnitPrice = od.UnitPrice,
                                        Discount = od.Discount,
                                        Quantity = od.Quantity,
                                        SubTotal = od.UnitPrice*od.Quantity*(1-(decimal)od.Discount)
                                    })
                                });
            return Ok(OrderRecord);
        }
        //會員參與活動記錄
        [HttpGet("ActRecord")]
        public IActionResult ActRecord()
        {
            var UserID = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var record = _context.SocialActivities.Include(s => s.Member)
                .ThenInclude(m => m.User).Where(m => m.Member.UserId == UserID).Select(s => new GetActRecordDto
                {
                    ActivityId = s.ActivityId,
                    ActivitiesName = s.ActivitiesName,
                    CreatedTime = s.CreatedTime,
                    EndTime = s.EndTime
                }).FirstOrDefault();

            return Ok(record);
        }
        
    }
}
