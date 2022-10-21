using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _3TeamProject.Areas.Sightseeings.Controllers
{
    [Authorize(Roles = ("Administrator, ChiefAdministrator, SuperAdministrator"))]
    [Route("Sightseeings/[controller]")]
    [ApiController]
    public class SightseeingController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllSightseeings() //TODO 景點介紹頁
        {
            return Ok();
        }
        [HttpGet]
        public IActionResult GetSightseeingsDetail() //TODO 景點詳細頁
        {
            return Ok();
        }
    }
}
