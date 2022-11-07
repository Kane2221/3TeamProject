using _3TeamProject.Areas.Administrators.Data;
using _3TeamProject.Areas.SocialActivities.Data;
using _3TeamProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Xml.Linq;

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
        //社群揪團頁(所有清單)
        [HttpGet("GetAllActivities")]
        public IActionResult GetAllActivities()
        {
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
            return Ok(Activities);
        }
        //社群揪團頁(登入帳號的記錄)
        [HttpGet("GetMyActivities")]
        public IActionResult GetMyActivities()
        {
            var UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var MyActivities = _context.SocialActivities.Include(s => s.Member).Where(m => m.Member.UserId == UserId).Select(s => new GetActivitiesDto
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
            return Ok(MyActivities);
        }
        //活動詳細內容頁
        [HttpGet("GetActivityDetail/{id}")]
        public IActionResult GetActivityDetail(int id)
        {
            var Activities = _context.SocialActivities.Include(s => s.Member)
                .Include(s => s.ActivitiesMessageBoards).Include(s => s.JoinMembers)
                .Where(s => s.ActivityId == id).Select(s => new GetActivitiesDetailDto
                {
                    ActivityId = s.ActivityId,
                    MemberName = s.Member.MemberName,
                    ActivitiesName = s.ActivitiesName,
                    ActivitiesAddress = s.ActivitiesAddress,
                    ActivitiesContent = s.ActivitiesContent,
                    CreatedTime = s.CreatedTime,
                    EndTime = s.EndTime,
                    LimitCount = s.LimitCount,
                    JoinCount = s.JoinCount,
                    ActitiesStartDate = s.ActitiesStartDate,
                    ActitiesFinishDate = s.ActitiesFinishDate,
                    ActivitiesMessageBoards = s.ActivitiesMessageBoards.Select(m => new GetActMsgBoardDto
                    {
                        ActivitiesMessageId = m.ActivitiesMessageId,
                        ActivitiesMessageContent=m.ActivitiesMessageContent,
                        ActivitiesCreatedDate=m.ActivitiesCreatedDate,
                        Account=m.User.Account
                    })
                });
            return Ok(Activities);
        }
        //活動發起頁
        [HttpPost("PostActivity")]
        public IActionResult PostActivity([FromBody] PostActDto post)
        {
            var UserID = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var member = _context.Members.FirstOrDefault(m => m.UserId == UserID);
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }

            SocialActivity Act = new SocialActivity
            {
                MemberId = member.MemberId,
                ActivitiesName = post.ActivitiesName,
                ActivitiesContent = post.ActivitiesContent,
                ActivitiesAddress = post.ActivitiesAddress,
                CreatedTime = DateTime.Now,
                EndTime = post.EndTime,
                LimitCount = post.LimitCount,
                JoinCount = 1,
                ActitiesStartDate = post.ActitiesStartDate,
                ActitiesFinishDate = post.ActitiesFinishDate
            };
            _context.SocialActivities.Add(Act);
            _context.SaveChanges();
            return Ok("活動已發起");
        }
        //活動留言
        [HttpPost("PostMessage")]
        public IActionResult PostMessage([FromBody] PostMessageDto post)
        {
            var UserID = int.Parse(User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Sid).Value);
            var message = new ActivitiesMessageBoard
            {
                ActivityId = post.ActivityId,
                ActivitiesMessageContent = post.ActivitiesMessageContent,
                ActivitiesCreatedDate = DateTime.Now,
                ActivitiesMessageState = 0
            };
            _context.ActivitiesMessageBoards.Add(message);
            _context.SaveChanges();
            return Ok("已留言");
        }
        //參與活動
        [HttpPost("JoinAct/{id}")]
        public IActionResult JoinAct(int id)
        {
            var UserID = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var user = _context.JoinMembers.Include(j=>j.Member).ThenInclude(m=>m.User).Where(j=>j.Member.UserId == UserID).SingleOrDefault();
            if (user != null)
            {
                return BadRequest("已參加此活動");
            }
            JoinMember join = new JoinMember
            {
                ActivitiesId = id,
                MemberId = UserID
            };
            var count = _context.JoinMembers.Where(j => j.ActivitiesId == id).Count();
            var act = _context.SocialActivities.Include(s => s.JoinMembers).Where(s => s.ActivityId == id).SingleOrDefault();
            act.JoinCount = count;
            _context.JoinMembers.Add(join);
            _context.SocialActivities.Update(act);
            _context.SaveChanges();
            return Ok("您已參與此活動");
        }
        //活動退出
        [HttpDelete("QuitAct/{id}")]
        public IActionResult QuitAct(int id)
        {
            var UserID = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var user = _context.JoinMembers.Include(j => j.Member).ThenInclude(m => m.User).Where(j => j.Member.UserId == UserID).SingleOrDefault();
            if (user == null)
            {
                return BadRequest("已退出此活動");
            }
            var quit = _context.JoinMembers.Where(j => j.Member.UserId == UserID).SingleOrDefault();
            _context.JoinMembers.Remove(quit);
            var count = _context.JoinMembers.Where(j => j.ActivitiesId == id).Count();
            var act = _context.SocialActivities.Include(s => s.JoinMembers).Where(s => s.ActivityId == id).SingleOrDefault();
            act.JoinCount = count;
            _context.SocialActivities.Update(act);
            _context.SaveChanges();
            return Ok("您已退出此活動");
        }
    }
}
