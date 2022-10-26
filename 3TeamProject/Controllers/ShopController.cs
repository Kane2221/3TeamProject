using _3TeamProject.Models;
using Microsoft.AspNetCore.Mvc;
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
            return View();
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

            Cart cart = new Cart
            {
                Id = id,
                Amount = 1
            };
            List<Cart> cartList = new List<Cart>();
            cartList.Add(cart);
            string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(cart);
            session.SetString("cart", jsonstring);

            return NoContent();
        }
        public IActionResult RemoveCart(int id)
        {
            ISession session = this.HttpContext.Session;
            //session.Remove(session,"cart");

            return NoContent();
        }
        public IActionResult Checkout()
        {
            return View();
        }

    }
}
