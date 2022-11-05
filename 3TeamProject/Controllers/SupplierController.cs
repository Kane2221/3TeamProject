using _3TeamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _3TeamProject.Controllers
{
    [Route("/Supplier/[Action]")]
    public class SupplierController : Controller
    {
        private _3TeamProjectContext _context;
        public SupplierController(_3TeamProjectContext context)
        {
            this._context = context;
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult ForgetPwd()
        {
            return View();
        }
        public IActionResult SupplierProfile()
        {
            return View();
        }
        public IActionResult ProductList()
        {
            return View();
        }
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost("/Supplier/AddOrder")]
        public async Task<JsonResult> AddOrder([FromBody] Product product)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return Json("新增成功!");
        }
        public IActionResult Verify()
        {
            return View();
        }
    }
}
