using Microsoft.AspNetCore.Mvc;

namespace _3TeamProject.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
