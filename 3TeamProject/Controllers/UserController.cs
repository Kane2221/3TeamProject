﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using _3TeamProject.Models;
using _3TeamProject.Data;

namespace _3TeamProject.Controllers
{
    public class UserController : Controller
    {
        private readonly _3TeamProjectContext _context;

        public UserController(_3TeamProjectContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Login([FromBody] LoginReuqest request)
        {
            var user = await _context.Users.Include(u => u.RolesNavigation).FirstOrDefaultAsync(u => u.Account == request.Account);
            var UserRole = user.RolesNavigation.RoleName;
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
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Account),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Role, UserRole)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPricipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPricipal);
            return Ok("登入成功");
            //return RedirectToAction("index", "home");
        }
        //TODO 新增寄送Token給使用者作驗證
        public async Task<IActionResult> Verify([FromBody] string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerficationToken == token);
            if (user == null)
            {
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
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
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
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }
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
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Logout");
            //return RedirectToAction("Login", "home");
        }
    }
}
