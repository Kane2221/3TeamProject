using _3TeamProject.Data;
using _3TeamProject.Extensions;
using _3TeamProject.Extensions.Util;
using _3TeamProject.Helpers;
using _3TeamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace _3TeamProject.Controllers
{
    //[Route("Shop/[Action]")]
    public class ShopController : Controller
    {
        private readonly IHostEnvironment environment;
        private _3TeamProjectContext _context;
        private BankInfoModel _bankInfoModel = new BankInfoModel
        {
            //MerchantID = "MS144606325",
            //HashKey = "T5iAuriSnKw7g7o2o477WZlSAitdS17F",
            //HashIV = "Ci2lec2SHxLfCyPP",
            ReturnURL = "https://localhost:7007/Shop/SpgatewayReturn",
            NotifyURL = "http://yourWebsitUrl/Bank/SpgatewayNotify",
            CustomerURL = "http://yourWebsitUrl/Bank/SpgatewayCustomer",
            AuthUrl = "https://ccore.spgateway.com/MPG/mpg_gateway",
            CloseUrl = "https://core.newebpay.com/API/CreditCard/Close"
        };
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
                ViewBag.Quantity = CartItem.Sum(n => n.Quantity);
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
                Quantity = 1,
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
                    cart[index].Quantity += item.Quantity;
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
        [HttpGet("/Shop/Checkout")]
        public IActionResult Checkout()
        {
            ISession session = this.HttpContext.Session;
            List<CartSessionDto> CartItem = SessionHelper.GetObjectFromJson<List<CartSessionDto>>(session, "cart");

                if (CartItem == null)
                {
                    return RedirectToAction("Cart");
                }
                else
                {
                    PayDto payDto = new PayDto
                    {
                        SubTotal = CartItem.Sum(n => n.SubTotal),
                        //ordernumber = DateTime.Now.ToString("yyyyMMdd") + i.ToString() ,
                        //MemberId = 1,
                        //AdministratorId=1,
                        //OrderDate = DateTime.Now,
                        //ShipDate = DateTime.Now,
                    };
                    ViewBag.Total = payDto.SubTotal;
                }
                return View();

        }
        [HttpPost("/Shop/Checkout")]
        public IActionResult Checkout([FromBody] PayDto request)
        {
            ISession session = this.HttpContext.Session;
            List<CartSessionDto> CartItem = SessionHelper.GetObjectFromJson<List<CartSessionDto>>(session, "cart");
            var order = new Order
            {
                //OrderId = int.Parse(DateTime.Now.ToString("yyyyMMdd") + i.ToString()),
                MemberId = 2,
                AdministratorId = 8,
                OrderDate = DateTime.Now,
                ShipDate = DateTime.Now,
                PaymentStatus = 0,
                ShipStatus = 0,
                ShipPostalCode = request.ShipPostalCode,
                ShipCountry = request.ShipCountry,
                ShipCity = request.ShipCity,
                ShipAddress = request.ShipAddress
            };
            foreach (var odi in CartItem)
            {
                var pic = new OrderDetail
                {
                    ProductId = odi.ProductId,
                    UnitPrice = odi.SubTotal,
                    Discount = 0,
                    Quantity = odi.Quantity
                };
                order.OrderDetails.Add(pic);
            }
            _context.Orders.Add(order);
            _context.SaveChanges();

            Pay(request);
            //return RedirectToAction("Pay");
            return Ok();
        }

        public IActionResult Rating()
        {
            return View();
        }

        public IActionResult Pay(PayDto payDto)
        {
            string version = "1.5";
            IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
            TradeInfo tradeInfo = new TradeInfo()
            {
                // * 商店代號
                MerchantID = Config.GetSection("PayConfig").GetSection("MerchantID").Value,
                // * 回傳格式
                RespondType = "String",
                // * TimeStamp
                TimeStamp = DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0)).ToUnixTimeSeconds().ToString(),
                // * 串接程式版本
                Version = version,
                // * 商店訂單編號
                //MerchantOrderNo = $"T{DateTime.Now.ToString("yyyyMMddHHmm")}",
                MerchantOrderNo = payDto.ordernumber,
                //MerchantOrderNo = "A45645641a1a1",
                // * 訂單金額
                //Amt = payDto.amount,
                Amt=payDto.SubTotal,
                // * 商品資訊
                ItemDesc = "商品資訊(自行修改)",
                // 支付完成 返回商店網址
                ReturnURL = _bankInfoModel.ReturnURL,
                // 支付通知網址
                NotifyURL = _bankInfoModel.NotifyURL,
                // 商店取號網址
                CustomerURL = _bankInfoModel.CustomerURL,
                // * 付款人電子信箱
                Email = payDto.Email,
                // 付款人電子信箱 是否開放修改(1=可修改 0=不可修改)
                EmailModify = 0,
                // 商店備註
                OrderComment = null,
                // 信用卡 一次付清啟用(1=啟用、0或者未有此參數=不啟用)
                CREDIT = 1,
            };

            var inputModel = new SpgatewayInputModel
            {
                MerchantID = Config.GetSection("PayConfig").GetSection("MerchantID").Value,
                Version = version
            };
            string HashKey = Config.GetSection("PayConfig").GetSection("HashKey").Value;
            string HashIV = Config.GetSection("PayConfig").GetSection("HashIV").Value;
            var tradeQueryPara = string.Join("&", LambdaUtil.ModelToKeyValuePairList<TradeInfo>(tradeInfo).Select(x => $"{x.Key}={x.Value}"));
            // AES 加密
            inputModel.TradeInfo = CryptoUtil.EncryptAESHex(tradeQueryPara, HashKey, HashIV);
            // SHA256 加密
            inputModel.TradeSha = CryptoUtil.EncryptSHA256($"HashKey={HashKey}&{inputModel.TradeInfo}&HashIV={HashIV}");

            // 將model 轉換為List<KeyValuePair<string, string>>, null值不轉
            var postData = LambdaUtil.ModelToKeyValuePairList<SpgatewayInputModel>(inputModel);
            //將5個欄位放入並以form submit方式送出
            StringBuilder s = new StringBuilder();
            s.Append("<html>");
            //<body onload='document.forms[\"form\"].submit()'>  ->onload的時候就自己submit一次
            s.AppendFormat("<body onload='document.forms[\"form\"].submit()'>");
            s.AppendFormat("<form name='form' action='{0}' method='post'>", _bankInfoModel.AuthUrl);
            foreach (KeyValuePair<string, string> item in postData)
            {
                s.AppendFormat("<input type='hidden' name='{0}' value='{1}' />", item.Key, item.Value);
            }

            s.Append("</form></body></html>");

            //HttpContext.Response.WriteAsync(s.ToString());
            //回傳Content s內html字串 指定為 "text/html" 格式
            return Content(s.ToString(), "text/html");
            //return View();
        }
        public ActionResult SpgatewayReturn()
        {
            //   Request.LogFormData("SpgatewayReturn(支付完成)");

            // Status 回傳狀態 
            // MerchantID 回傳訊息
            // TradeInfo 交易資料AES 加密
            // TradeSha 交易資料SHA256 加密
            // Version 串接程式版本
            StringBuilder receive = new StringBuilder();
            foreach (var item in Request.Form)
            {
                receive.AppendLine(item.Key + "=" + item.Value + "<br>");
            }
            //NameValueCollection collection = Request.Form;

            if ( string.Equals(Request.Form["MerchantID"], _bankInfoModel.MerchantID) &&
                 string.Equals(Request.Form["TradeSha"], CryptoUtil.EncryptSHA256($"HashKey={_bankInfoModel.HashKey}&{Request.Form["TradeInfo"]}&HashIV={_bankInfoModel.HashIV}")))
            {
                var decryptTradeInfo = CryptoUtil.DecryptAESHex(Request.Form["TradeInfo"], _bankInfoModel.HashKey, _bankInfoModel.HashIV);

                // 取得回傳參數(ex:key1=value1&key2=value2),儲存為NameValueCollection
                NameValueCollection decryptTradeCollection = HttpUtility.ParseQueryString(decryptTradeInfo);
                SpgatewayOutputDataModel convertModel = LambdaUtil.DictionaryToObject<SpgatewayOutputDataModel>(decryptTradeCollection.AllKeys.ToDictionary(k => k, k => decryptTradeCollection[k]));

               // LogUtil.WriteLog(JsonConvert.SerializeObject(convertModel));

                // TODO 將回傳訊息寫入資料庫

                return Content(JsonConvert.SerializeObject(convertModel));
            }
            else
            {
              //  LogUtil.WriteLog("MerchantID/TradeSha驗證錯誤");
            }

            return Content(string.Empty, "text/html");
        }



    }
}
