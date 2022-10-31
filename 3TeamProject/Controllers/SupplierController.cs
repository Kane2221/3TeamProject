using _3TeamProject.Data;
using _3TeamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _3TeamProject.Controllers
{
    [Route("/Supplier/[Action]")]
    public class SupplierController : Controller
    {
        private IHostEnvironment environment;
        private _3TeamProjectContext _context;
        public SupplierController(_3TeamProjectContext context, IHostEnvironment environment)
        {
            this._context = context;
            this.environment = environment;
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
        public IActionResult OrderList()
        {
            return View();
        }
        [HttpGet("/Supplier/AddOrder")]
        public IActionResult AddOrder()
        {
            return View();
        }

        [HttpPost("/Supplier/AddOrder")]
        public async Task<JsonResult> AddOrder([FromBody] AddOrderDto addOrder)
        {
            foreach (var file in addOrder.files)
            {
                var root = $@"{environment.ContentRootPath}\wwwroot\";
                var temp = "";
                if (file.FileName.Contains(".png"))
                {
                    temp = root + "picture";
                }
                else
                {
                    temp = root + "other";
                }
                var path = temp + "\\" + DateTime.Now.Ticks.ToString() + file.FileName;
                using (var fs = System.IO.File.Create(path))
                    file.CopyTo(fs);
            }
                Product product = new Product
            {
                ProductCategoryId = addOrder.ProductCategoryId,
                ProductName = addOrder.ProductName,
                UnitStock = addOrder.UnitStock,
                AddedTime = DateTime.Now,
                SupplierId = addOrder.SupplierId,
                ProductUnitPrice = addOrder.ProductUnitPrice,
                ProductStatusId = addOrder.ProductStatusId,
                ProductIntroduce = addOrder.ProductIntroduce,

            };

            _context.Add(product).CurrentValues.SetValues(addOrder);
            await _context.SaveChangesAsync();
            return Json("新增成功!");
        }
    }
}
