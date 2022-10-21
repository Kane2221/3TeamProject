using _3TeamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;
<<<<<<< HEAD
//using static _3TeamProject.Models.MemberModel;
=======
>>>>>>> e1af6f3e8a20b9717349e0b46dfb6f09d329bbe6
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

