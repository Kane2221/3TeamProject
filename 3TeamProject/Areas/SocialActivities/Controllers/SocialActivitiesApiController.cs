using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _3TeamProject.Areas.SocialActivities.Controllers
{
    [Authorize]
    [Route("SocialActivities/[controller]")]
    [ApiController]
    public class SocialActivitiesApiController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllActivities() //TODO 社群揪團頁
        {
            return Ok();
        }
        [HttpGet("{id}")]
        public IActionResult GetActivityDetail() //TODO 活動詳細內容頁
        {
            return Ok();
        }
        [HttpPost]
        public IActionResult PublishActivity() //TODO 活動發起頁
        {
            return Ok();
        }
        [HttpPost]
        public IActionResult LeaveMessage() //TODO 活動留言頁
        {
            return Ok();
        }

    }
}
