using Microsoft.AspNetCore.Mvc;

namespace _3TeamProject.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
