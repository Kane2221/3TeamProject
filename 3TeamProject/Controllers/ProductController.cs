﻿using _3TeamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public async Task<IActionResult> Detail([FromForm] int post)
        {
            var id = post;
            //var id = Int32.Parse(post["id"].ToString());

            var arf = 456;
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
            var productFound = from product in _context.Products
                               join Info in _context.ProductsPictureInfos
                               on product.ProductId equals Info.ProductId
                               join Board in _context.ProductsMessageBoards
                               on product.ProductId equals Board.ProductId
                               where product.ProductId == id
                               select new
                               {
                                   ProductId = product.ProductId,
                                   ProductCategoryId = product.ProductCategoryId,
                                   ProductName = product.ProductName,
                                   ProductUnitPrice = product.ProductUnitPrice,
                                   SupplierId = product.SupplierId,
                                   QuantityPerUnit = product.QuantityPerUnit,
                                   UnitStock = product.UnitStock,
                                   ProductRecommendation = product.ProductRecommendation,
                                   ProductStatus = product.ProductStatus,
                                   ProductPicturePath = Info.ProductPicturePath,    
                                   ProductMessageContent = Board.ProductMessageContent,
                                   ProductIntroduce = product.ProductIntroduce
                                   //p.ProductName,
                                   //p.ProductCategoryId

                               };
            
            return View(productFound);
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
