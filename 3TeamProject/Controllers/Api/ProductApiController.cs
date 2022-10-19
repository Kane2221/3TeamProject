using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _3TeamProject.Models;
using Microsoft.CodeAnalysis;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace _3TeamProject.Controllers.Api
{
    [Route("api/Product")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly _3TeamProjectContext _context;

        public ProductApiController(_3TeamProjectContext context)
        {
            _context = context;
        }

        // GET: api/Products
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        //{
        //    return await _context.Products.ToListAsync();
        //}

        //// GET: api/Products/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Product>> GetProduct(int id)
        //{
        //    var product = await _context.Products.FindAsync(id);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return product;
        //}

        //// PUT: api/Products/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutProduct(int id, Product product)
        //{
        //    if (id != product.ProductId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(product).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProductExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Products
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Product>> PostProduct(Product product)
        //{
        //    _context.Products.Add(product);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        //}

        //// DELETE: api/Products/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProduct(int id)
        //{
        //    var product = await _context.Products.FindAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Products.Remove(product);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool ProductExists(int id)
        //{
        //    return _context.Products.Any(e => e.ProductId == id);
        //}

        [HttpGet]
        public ActionResult<List<Product>> Get()
        {
            //var productFound = _context.Products.Join
            //  (_context.ProductsPictureInfos,
            //  p => p.ProductId,
            //  i => i.ProductId,
            //  (p, i) => new
            //  {
            //      ProductId = p.ProductId,
            //      //p.ProductName,
            //      //p.ProductCategoryId

            //  });
            var productFound = (from product in _context.Products 
                               join Info in _context.ProductsPictureInfos 
                               on product.ProductId equals Info.ProductId
                               orderby product.ProductId
                               select new
                               {
                                   ProductId = product.ProductId,
                                   ProductCategoryId = product.ProductCategoryId,
                                   ProductName = product.ProductName,
                                   ProductUnitPrice=product.ProductUnitPrice,
                                   ProductPicturePath = Info.ProductPicturePath

                               });
            var aa = _context.Products.Include(p => p.ProductsPictureInfos).ToList();
            return Ok(productFound);
            //return _context.Products;
        }


        // GET api/<ProductApiController>/5
        [HttpGet("{id}")]
        //public Models.Product Get(int id)
        public ActionResult<List<Product>> Get(int id)
        {
            //var productFound =  _context.Products.Join
            //   (_context.ProductsPictureInfos,
            //   p => p.ProductId,
            //   i => i.ProductId,
            //   (p, i) => new
            //   {
            //       ProductId= p.ProductId,
            //       ProductCategoryId=p.ProductCategoryId,
            //       ProductName= p.ProductName,
            //       ProductUnitPrice=p.ProductUnitPrice,
            //       SupplierId =p.SupplierId,
            //       QuantityPerUnit=p.QuantityPerUnit,
            //       UnitStock=p.UnitStock,
            //       ProductRecommendation=p.ProductRecommendation,
            //       ProductStatus=p.ProductStatus,
            //       ProductPicturePath = i.ProductPicturePath
            //       //p.ProductName,
            //       //p.ProductCategoryId

            //   }).Where(pi=>pi.ProductId== id);
            var productFound = from product in _context.Products
                               join Info in _context.ProductsPictureInfos
                               on product.ProductId equals Info.ProductId
                               join Board in _context.ProductsMessageBoards                             
                               on product.ProductId equals Board.ProductId
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
            return Ok(productFound);//_context.Products.Find(productFound);
            //return ((Task<IActionResult>)productFound);
        }

        // POST api/<ProductApiController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductApiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductApiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        //public async Task<JsonResult> Filter([FromBody] Product product)
        //{
        //    var productFound = await _context.Products.Join
        //        (_context.ProductsPictureInfos,
        //        p=>p.ProductId,
        //        i=>i.ProductId,
        //        (p, i) => new
        //        {
        //            p.ProductId,
        //            p.ProductName,
        //            p.ProductCategoryId

        //        }).ToListAsync();

        //    return Json(productFound);
        //}
    }
}
