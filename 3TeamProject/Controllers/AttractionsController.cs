using Microsoft.AspNetCore.Mvc;

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
            return View();
        }
    }
}
