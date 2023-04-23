using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _3TeamProject.Controllers
{
    public class ActivityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult ActivityDetail(int id)
        {
            return View();
        }
        [Authorize]
        public IActionResult AddActivity()
        {
            return View();
        }
    }
}
