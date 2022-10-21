using Microsoft.AspNetCore.Mvc;

namespace _3TeamProject.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Cart()
        {
            return View();
        }
        public IActionResult ProductDetail()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
