using _3TeamProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace _3TeamProject.Controllers
{
    public class ShopController : Controller
    {
        private readonly IHostEnvironment environment;
        private _3TeamProjectContext _context;

        public ShopController(_3TeamProjectContext context)
        {
            this._context = context;
        }
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
        public IActionResult Rating()
        {
            return View();
        }
    }
}
