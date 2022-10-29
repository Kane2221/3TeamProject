using _3TeamProject.Data;
using _3TeamProject.Helpers;
using _3TeamProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace _3TeamProject.Controllers
{
    public class ShopController : Controller
    {
        private readonly IHostEnvironment environment;
        private _3TeamProjectContext _context;
        
        public ShopController(_3TeamProjectContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Cart()
        {
            ISession session = this.HttpContext.Session;
            //var c = session.GetString("cart");
            //var pid = session.GetString("ProductId");
            List<CartSessionDto> CartItem = SessionHelper.GetObjectFromJson<List<CartSessionDto>>(session, "cart");
            
            if(CartItem!= null)
            {
                ViewBag.Total = CartItem.Sum(n => n.SubTotal);
                ViewBag.Amount = CartItem.Sum(n => n.Amount);
                //CartItem = new List<Cart>()
            }
            else
            {
                ViewBag.Total = 0;
            }
            return View(CartItem);
        }
        public IActionResult ProductDetail(int id)
        {
            var pid = id;
            ViewBag.ProductId = pid;
            

            return View();
        }
        public IActionResult AddtoCart(int id)
        {
            ISession session = this.HttpContext.Session;

            CartSessionDto item = new CartSessionDto
            {
                ProductId = id,
                Amount = 1,
                SubTotal = (int)_context.Products.Single(n => n.ProductId == id).ProductUnitPrice
                           //Int32.Parse(from p in _context.Products where p.ProductId == id select p.UnitStock),
                
            };

            if (SessionHelper.GetObjectFromJson<List<CartSessionDto>>(session, "cart") == null)//session內沒有購物車
            {
                List<CartSessionDto> cart = new List<CartSessionDto>();
                cart.Add(item);
                SessionHelper.SetObjectAsJson(session, "cart", cart);
            }
            else
            {
                List<CartSessionDto> cart = SessionHelper.GetObjectFromJson<List<CartSessionDto>>(session, "cart");
                int index = cart.FindIndex(n => n.ProductId.Equals(id));
                //int index = cart.Find(id);
                if (index !=-1)
                {
                    cart[index].Amount += item.Amount;
                    cart[index].SubTotal += item.SubTotal;
                }
                else
                {
                    cart.Add(item);
                }
                SessionHelper.SetObjectAsJson(session, "cart", cart);
            }
            
            //string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(item);
            //session.SetString("cart", jsonstring);

            return NoContent();
        }
        public IActionResult RemoveCart(int id)
        {
            ISession session = this.HttpContext.Session;
            List<CartSessionDto> cart = SessionHelper.GetObjectFromJson<List<CartSessionDto>>(session, "cart");

            int index = cart.FindIndex(n => n.ProductId.Equals(id));
            cart.RemoveAt(index);
            if(cart.Count<1)
            {
                SessionHelper.Remove(session, "cart");
            }
            else
            {
                SessionHelper.SetObjectAsJson(session, "cart", cart);
            }

            //return NoContent();
            return RedirectToAction ("Cart");
        }
        public IActionResult Checkout()
        {
            ISession session = this.HttpContext.Session;
            if(SessionHelper.GetObjectFromJson<List<CartSessionDto>>(session,"cart")==null)
            {
                return RedirectToAction("Cart");
            }
            else
            {
                var setorder = new Order
                {
                    MemberId = 1,
                    AdministratorId=1,
                    OrderDate = DateTime.Now,
                    ShipDate = DateTime.Now,
                };
            }
            return View();
        }

        public IActionResult Rating()
        {
            return View();
        }

    }
}
