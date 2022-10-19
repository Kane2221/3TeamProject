using _3TeamProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _3TeamProject.Controllers.Api
{

    [Route("api/ProductsPictureInfos")]
    [ApiController]
    public class ProductsPictureInfoApiController : ControllerBase
    {
        private readonly _3TeamProjectContext _context;

        public ProductsPictureInfoApiController(_3TeamProjectContext context)
        {
            _context = context;
        }
        // GET: api/<ProductsPictureInfoApiController>
        [HttpGet]
        public IEnumerable<ProductsPictureInfo> Get()
        {
            return _context.ProductsPictureInfos;
            //return _context.ProductsPictureInfos;
        }

        // GET api/<ProductsPictureInfoApiController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductsPictureInfoApiController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductsPictureInfoApiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductsPictureInfoApiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
