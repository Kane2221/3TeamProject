using Microsoft.AspNetCore.Mvc;

namespace _3TeamProject.Controllers
{
    public class ActivityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ActivityDetail(int id)
        {
            return View();
        }
        public IActionResult AddActivity()
        {
            return View();
        }
    }
}
