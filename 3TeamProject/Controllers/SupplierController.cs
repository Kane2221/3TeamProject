using _3TeamProject.Data;
using _3TeamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _3TeamProject.Controllers
{
    //[Route("/Supplier/[Action]")]
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
        public IActionResult ProductList()
        {
            return View();
        }
        //[HttpGet("/Supplier/AddOrder")]
        [HttpGet]
        public IActionResult AddOrder()

        {
            return View();
        }
        [HttpPost]
        //[HttpPost("/Supplier/AddOrder")]
        public async Task<JsonResult> AddOrder([FromForm] AddOrderDto addOrder)
        {
            var addproduct = new Product
            {
                ProductCategoryId = addOrder.ProductCategoryId,
                ProductName = addOrder.ProductName,
                UnitStock = addOrder.UnitStock,
                AddedTime = DateTime.Now,
                SupplierId = 2,
                ProductUnitPrice = addOrder.ProductUnitPrice,
                ProductStatusId = addOrder.ProductStatusId,
                ProductIntroduce = addOrder.ProductIntroduce,

            };
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
                    temp = root + "img";
                }
                // var path = temp + "\\" + DateTime.Now.Ticks.ToString() + file.FileName;
                var path = temp + "\\"  + file.FileName;
                using (var fs = System.IO.File.Create(path))
                    file.CopyTo(fs);
                var prodpic = new ProductsPictureInfo
                {
                    ProductPicturePath ="/img/"+ file.FileName,
                    ProductPictureName = file.FileName
                };
                addproduct.ProductsPictureInfos.Add(prodpic);
            }
            

            //_context.Add(product).CurrentValues.SetValues(addOrder);
            _context.Add(addproduct);
            await _context.SaveChangesAsync();
            return Json("新增成功!");
        }
        public IActionResult Verify()
        {
            return View();
        }
    }
}
