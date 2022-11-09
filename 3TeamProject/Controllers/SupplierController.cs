using _3TeamProject.Areas.Suppliers.Data;
using _3TeamProject.Data;
using _3TeamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        public IActionResult OrderList()
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
        public async Task<IActionResult> AddOrder([FromForm] AddOrderDto addOrder)
        {
            var UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var supplier = _context.Suppliers.FirstOrDefault(x=>x.UserId == UserId);
            var addproduct = new Product
            {
                ProductCategoryId = addOrder.ProductCategoryId,
                ProductName = addOrder.ProductName,
                UnitStock = addOrder.UnitStock,
                AddedTime = DateTime.Now,
                SupplierId = supplier.SuppliersId,
                ProductUnitPrice = addOrder.ProductUnitPrice,
                ProductStatusId = 0,
                ProductIntroduce = addOrder.ProductIntroduce,

            };
            if (addOrder.files != null)
            {
                foreach (var file in addOrder.files)
                {
                    var root = $@"{environment.ContentRootPath}\wwwroot\";
                    var tempRoot = "";
                    if (file.FileName.Contains(".jpg"))
                    {
                        tempRoot = root +"img"+"\\"+"Sight"+"\\" +"picture"+ "\\" + addOrder.ProductName;
                    }
                    else
                    {
                        tempRoot = root +"img"+"\\"+"Sight"+"\\"+"other"+ "\\" +addOrder.ProductName;
                    }
                    if (!Directory.Exists(tempRoot))
                    {
                        Directory.CreateDirectory(tempRoot);
                    }
                    var path = tempRoot +"\\" + file.FileName;

                    file.CopyTo(System.IO.File.Create(path));
                    var prodpic = new ProductsPictureInfo
                    {
                        ProductPicturePath ="/img/"+ file.FileName,
                        ProductPictureName = file.FileName
                    };
                    addproduct.ProductsPictureInfos.Add(prodpic);
                }
            }
            //_context.Add(product).CurrentValues.SetValues(addOrder);
            _context.Add(addproduct);
            await _context.SaveChangesAsync();
            return RedirectToAction("OrderList");
        }
        public IActionResult Verify()
        {
            return View();
        }
        public IActionResult ResetPwd()
        {
            return View();
        }
    }
}
