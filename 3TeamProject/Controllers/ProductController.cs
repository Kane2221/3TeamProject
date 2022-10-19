using _3TeamProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace _3TeamProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHostEnvironment environment;
        private _3TeamProjectContext _context;
        public ProductController(_3TeamProjectContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        //[HttpGet("Detail/{id}")]
        public IActionResult Detail()
        {
            //var productFound = _context.Products.Join
            //(_context.ProductsPictureInfos,
            //   p => p.ProductId,
            //   i => i.ProductId,
            //   (p, i) => new
            //   {
            //       ProductId = p.ProductId,
            //       ProductCategoryId = p.ProductCategoryId,
            //       ProductName = p.ProductName,
            //       ProductUnitPrice = p.ProductUnitPrice
            //       //p.ProductName,
            //       //p.ProductCategoryId

            //   }).Where(pi => pi.ProductId == id);
            return View();
        }
        //[HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }


        //public void Upload(ProductsPictureInfo model)
        //{

        //    foreach (var file in model.ProductPictureId)
        //    {
        //        var root = $@"{environment.ContentRootPath}\wwwroot\";
        //        var temp = "";
        //        if (file.FileName.Contains(".png"))
        //        {
        //            temp = root + "picture";
        //        }
        //        else
        //        {
        //            temp = root + "other";
        //        }
        //        var path = temp + "\\" + DateTime.Now.Ticks.ToString() + file.FileName;
        //        using (var fs = System.IO.File.Create(path))
        //        {
        //            file.CopyTo(fs);
        //            var u = new ProductsPictureInfo.Services.User
        //            {
        //                Id = 1,
        //                Name = model.name,
        //                Path = "\\" + path.Replace(root, "")
        //            };
        //            MockDB.Users.Add(u);
        //        }
        //    }

        //}
    }
}
