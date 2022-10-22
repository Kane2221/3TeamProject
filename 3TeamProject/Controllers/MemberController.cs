using _3TeamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;


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
        public IActionResult ForgetPwd()
        {
            return View();
        }
        public IActionResult ResetPwd()
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

