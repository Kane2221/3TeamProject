using _3TeamProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _3TeamProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly _3TeamProjectContext _DbContext;

        public HomeController(ILogger<HomeController> logger, _3TeamProjectContext DbContext)
        {
            _logger = logger;
            _DbContext = DbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}