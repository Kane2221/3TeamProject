using _3TeamProject.Areas.Administrators.Data;
using _3TeamProject.Areas.SocialActivities.Data;
using _3TeamProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace _3TeamProject.Areas.SocialActivities.Controllers
{
    [Authorize]
    [Route("SocialActivities/[controller]")]
    [ApiController]
    public class SocialActivitiesApiController : ControllerBase
    {
        private readonly _3TeamProjectContext _context;

        public SocialActivitiesApiController(_3TeamProjectContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAllActivities() //TODO 社群揪團頁
        {
            var UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var user = _context.Users.Include(u => u.Members).FirstOrDefault(x => x.UserId == UserId);
            var Activities = _context.SocialActivities.Include(s => s.Member).Select(s => new GetActivitiesDto
            {
                ActivityId = s.ActivityId,
                MemberName = s.Member.MemberName,
                ActivitiesName = s.ActivitiesName,
                ActivitiesAddress = s.ActivitiesAddress,
                CreatedTime = s.CreatedTime,
                EndTime = s.EndTime,
                LimitCount = s.LimitCount,
                JoinCount = s.JoinCount
            });

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
