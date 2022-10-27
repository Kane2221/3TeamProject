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
            var pid = session.GetString("cart");
            List<Cart> CartItem = SessionHelper.GetObjectFromJson<List<Cart>>(session, "cart");
            
            if(CartItem!= null)
            {
               // ViewBag.Total = CartItem.Sum(n => n.SubTotal);
                //CartItem = new List<Cart>()
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

            Cart item = new Cart
            {
                ProductId = id,
                Amount = 1
            };

            if (SessionHelper.GetObjectFromJson<List<Cart>>(session, "cart") == null)//session內沒有購物車
            {
                List<Cart> cart = new List<Cart>();
                cart.Add(item);
                SessionHelper.SetObjectAsJson(session, "cart", cart);
            }
            else
            {
                List<Cart> cart = SessionHelper.GetObjectFromJson<List<Cart>>(session, "cart");
                int index = cart.FindIndex(n => n.ProductId.Equals(id));
                //int index = cart.Find(id);
                if (index !=-1)
                {
                    cart[index].Amount += item.Amount;
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
            List<Cart> cart = SessionHelper.GetObjectFromJson<List<Cart>>(session, "cart");

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

            return NoContent();
        }
        public IActionResult Checkout()
        {
            ISession session = this.HttpContext.Session;
            if(SessionHelper.GetObjectFromJson<List<Cart>>(session,"cart")==null)
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

    }
}
