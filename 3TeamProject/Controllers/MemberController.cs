using _3TeamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace _3TeamProject.Controllers
{
    public class MemberController : Controller
    {
        //        
        public IActionResult Register()
        {
            return View();
        }
        
        public IActionResult Memberprofile()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult GoogleLogin()
        {
            AuthenticationProperties authentication = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("LoginResult")

            };
            return Challenge(authentication, GoogleDefaults.AuthenticationScheme);
        }
        public async Task<IActionResult> LoginResult()
        {
            var token = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var result = token.Principal.Claims.Select(value => new
            {
                value.Value,
                value.Type,
                value.Issuer,
                value.Properties

            });
            //return Json(result);
            return Redirect("/Home");
        }
        public IActionResult ForgetPwd()
        {
            return View();
        }
        public IActionResult ResetPwd()
        {
            return View();
        }
        public IActionResult Verify()
        {
            return View();
        }
        public IActionResult MyOrder()
        {
            return View();
        }
        public IActionResult OrderRecord()
        {
            return View();
        }
        public IActionResult ActRecord()
        {
            return View();
        }

    }
}

