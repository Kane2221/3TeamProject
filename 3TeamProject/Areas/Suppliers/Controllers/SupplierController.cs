
﻿using _3TeamProject.Areas.Sppliers.Data;
using _3TeamProject.Models;
using _3TeamProject.Areas.Suppliers.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;


namespace _3TeamProject.Areas.Sppliers.Controllers
{
    [Area("Suppliers")]
    public class SupplierController : Controller
    {
        private readonly _3TeamProjectContext _context;

        private readonly IConfiguration _config;

        public SupplierController(_3TeamProjectContext Context, IConfiguration config)
        {
            _context = Context;
            _config = config;
        }

        [Authorize(Roles = ("Suppliers"))]
        public IActionResult GetSupplier()
        {
            var UserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value;
            var user = _context.Users.Include(u => u.Suppliers).FirstOrDefault(x => x.UserId == int.Parse(UserId));
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

        [Authorize(Roles = ("Administrator, ChiefAdministrator, SuperAdministrator"))]
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

        public async Task<IActionResult> Register([FromBody]SupplierRequestViewModel request)
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

                //Supplier supplier = new Supplier
                //{
                //    ContactName = request.ContactName,
                //    CompanyName = request.CompanyName,
                //    TaxId = request.TaxId,
                //    Fax = request.Fax,
                //    CellPhoneNumber = request.CellPhoneNumber,
                //    SupplierPhoneNumber = request.SupplierPhoneNumber,
                //    SupplierPostalCode = request.SupplierPostalCode,
                //    SupplierCountry = request.SupplierCountry,
                //    SupplierCity = request.SupplierCity,
                //    SupplierAddress = request.SupplierAddress,
                //    IdNavigation = new User
                //    {
                //        Account = request.Account,
                //        Email = request.Email,
                //        PasswordHash = passwordHsah,
                //        PasswordSalt = passwordSalt,
                //        VerficationToken = verifyToken,
                //        Roles = request.Roles
                //    }
                //};
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
                //using (MailMessage mail = new MailMessage())
                //{
                //    mail.From = new MailAddress("dotnettgm102@gmail.com", "帳號驗證碼");
                //    mail.To.Add("dotnettgm102@gmail.com");
                //    mail.Priority = MailPriority.Normal;
                //    mail.Subject = "帳號驗證碼";
                //    mail.Body = $"<a href=\"https://localhost:7190/User/Verify\"  value=\"{verifyToken}\">帳號驗證碼</a>";
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

        [Authorize(Roles = ("Suppliers, Administrator, ChiefAdministrator, SuperAdministrator"))]
        public async Task<IActionResult> Update(int? id, [FromBody]SupplierUpdateViewModel request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }
            var supplier = _context.Suppliers.Include(s => s.User).Where(s => s.UserId == id)
                .Select(s => s).SingleOrDefault();
            if (supplier == null)
            {
                return BadRequest("此帳號不存在");
            }
            //var UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
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
        public async Task<IActionResult> Delete(int id)
        {
            var user = _context.Users.Include(u => u.Suppliers).FirstOrDefault(x => x.UserId == id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("此帳號已刪除");
        }
    }
}
