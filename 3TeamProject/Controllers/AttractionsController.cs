using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace _3TeamProject.Controllers
{
    public class AttractionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AttractionsDetail(int id)
        {
            var pid = id;
            ViewBag.Sight = pid;
            return View();
        }
    }
}
