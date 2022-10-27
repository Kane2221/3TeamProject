using _3TeamProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _3TeamProject.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly _3TeamProjectContext _context;

        public CartApiController(_3TeamProjectContext context)
        {
            _context = context;
        }   
        public ActionResult<List<Product>>Get()
        {
            return Ok();
        }
    }
}
