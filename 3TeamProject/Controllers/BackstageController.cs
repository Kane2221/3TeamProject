using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _3TeamProject.Controllers
{
    //[Authorize(Policy = "AdminPolicy")]
    [Authorize(Roles = ("Administrator, ChiefAdministrator, SuperAdministrator"))]
    public class BackstageController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Admin()
        {
            return View();
        }
        public IActionResult Member()
        {
            return View();
        }
        public IActionResult Supplier()
        {
            return View();
        }
        public IActionResult Product()
        {
            return View();
        }
        public IActionResult Order()
        {
            return View();
        }
        public IActionResult Attraction()
        {
            return View();
        }
        public IActionResult AddAttraction()
        {
            return View();
        }
        public IActionResult Community()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
    }
}
