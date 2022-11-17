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
            //var quantity = CartItem.Select(q=>q.Quantity).ToList();
            //var subTotal = CartItem.Select(m=>m.SubTotal).ToList();

            var cart = _context.Products.Include(x => x.ProductsPictureInfos)
                .Where(x => ids.Contains(x.ProductId)).ToList()
                .Select(x => new CartApiDto
                {
                    ProductId = x.ProductId,
                    UnitStock = x.UnitStock,
                    ProductName = x.ProductName,
                    ProductUnitPrice = x.ProductUnitPrice,
                    ProductPicturePath = x.ProductsPictureInfos.FirstOrDefault().ProductPicturePath,
                    Amount = CartItem.FirstOrDefault(z=>x.ProductId == z.ProductId).Quantity,
                    SubTotal = CartItem.FirstOrDefault(z => x.ProductId == z.ProductId).SubTotal
                    

                });

            return Ok(cart);
        }
    }
}
