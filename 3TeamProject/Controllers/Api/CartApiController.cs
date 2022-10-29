using _3TeamProject.Data;
using _3TeamProject.Helpers;
using _3TeamProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace _3TeamProject.Controllers.Api
{
    [Route("api/Cart")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly _3TeamProjectContext _context;

        public CartApiController(_3TeamProjectContext context)
        {
            _context = context;
        }
        public ActionResult<List<CartSessionDto>> Get()
        {
            ISession session = this.HttpContext.Session;
            List<CartSessionDto> CartItem = SessionHelper.GetObjectFromJson<List<CartSessionDto>>(session, "cart");

            //var C = CartItem.Count;
            //var temp= CartItem[0].ProductId;
            var ids = CartItem.Select(x => x.ProductId).ToList();
            var t = CartItem.Select(q=>q.Amount).ToList();

            var cart= _context.Products.Include(x => x.ProductsPictureInfos)
                .Where(x => ids.Contains(x.ProductId))
                .Select(x => new CartApiDto
                {
                    ProductId = x.ProductId,
                    UnitStock = x.UnitStock,
                    ProductName = x.ProductName,
                    ProductUnitPrice = x.ProductUnitPrice,
                    ProductPicturePath = x.ProductsPictureInfos.FirstOrDefault().ProductPicturePath,
                    Amount = t.Count,
                    //SubTotal = CartItem.Sum(n => n.SubTotal)

                });




            //var cart = from p in _context.Products
            //           join i in _context.ProductsPictureInfos
            //           on p.ProductId equals i.ProductId
            //           where p.ProductId== CartItem.First().ProductId
            //           select new CartApiDto
            //           {
            //               ProductId = p.ProductId,
            //               UnitStock = p.UnitStock,
            //               ProductName = p.ProductName,
            //               ProductUnitPrice = p.ProductUnitPrice,
            //               ProductPicturePath = i.ProductPicturePath,
            //               Amount = CartItem.First().Amount,
            //               //SubTotal = CartItem.Sum(n => n.SubTotal)
            //           };
            //var cartinfo = from product in _context.Products
            //               join cart in Models.Cart
            return Ok(cart);
        }
    }
}
