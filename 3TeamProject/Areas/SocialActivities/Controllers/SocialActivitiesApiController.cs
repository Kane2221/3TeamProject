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
        public readonly IConfiguration _config;
        public IHostEnvironment _env;
        public SocialActivitiesApiController(_3TeamProjectContext context, IConfiguration config, IHostEnvironment env)
        {
            _context = context;
            _config = config;
            _env = env;
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
                CreatedTime = s.CreatedTime.ToString("d"),
                EndTime = s.EndTime.ToString("d"),
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
            var Member = _context.Users.Include(u => u.Members).FirstOrDefault(x => x.UserId == UserId);
            if (Member == null)
            {
                return BadRequest("還未登入帳號");
            }
            var MyActivities = _context.SocialActivities.Include(s => s.Member).Where(m => m.Member.UserId == UserId).Select(s => new GetActivitiesDto
            {
                ActivityId = s.ActivityId,
                MemberName = s.Member.MemberName,
                ActivitiesName = s.ActivitiesName,
                ActivitiesAddress = s.ActivitiesAddress,
                CreatedTime = s.CreatedTime.ToString("d"),
                EndTime = s.EndTime.ToString("d"),
                LimitCount = s.LimitCount,
                JoinCount = s.JoinCount
            });
            return Ok(MyActivities);
        }
        //活動詳細內容頁
        [HttpGet("GetActivityDetail/{id}")]
        public async Task<IActionResult> GetActivityDetail(int id)
        {
            var Activities = _context.SocialActivities.Include(s => s.Member)
                .Include(s => s.ActivitiesMessageBoards)
                .Where(s => s.ActivityId == id).SingleOrDefault();
            if (Activities == null)
            {
                return BadRequest("無此景點");
            }
            
            string baseUrl = "https://maps.googleapis.com/maps/api/staticmap";
            string center = Activities.ActivitiesAddress;
            int zoom = 14;
            int width = 640;
            int height = 480;
            string apiKey = _config["GoogleMap:KEY"];
            var root = $@"{_env.ContentRootPath}\wwwroot\";
            var tempRoot = "";
            tempRoot = root + "img" + "\\" + "map";
            string url = $"{baseUrl}?center={center}&format=jpg&zoom={zoom}&size={width}x{height}&key={apiKey}";
            var path = tempRoot + "\\" + Activities.ActivitiesAddress + ".jpg";
            if (!Directory.Exists(tempRoot))
            {
                Directory.CreateDirectory(tempRoot);
            }
            if (!System.IO.File.Exists(path))
            {
                using (HttpClient client = new HttpClient())
                {
                    byte[] response = await client.GetByteArrayAsync(url);
                    System.IO.File.WriteAllBytes(path, response);
                }
            }

            var res = new GetActivitiesDetailDto
            {
                ActivityId = Activities.ActivityId,
                MemberName = Activities.Member.MemberName,
                ActivitiesName = Activities.ActivitiesName,
                ActivitiesAddress = Activities.ActivitiesAddress,
                ActivitiesContent = Activities.ActivitiesContent,
                CreatedTime = Activities.CreatedTime,
                EndTime = Activities.EndTime,
                LimitCount = Activities.LimitCount,
                JoinCount = Activities.JoinCount,
                ActitiesStartDate = Activities.ActitiesStartDate,
                ActitiesFinishDate = Activities.ActitiesFinishDate,
                ActivityPictureName = Activities.ActivitiesAddress,
                ActivityPicturePath = "\\" + path.Replace(root, ""),
                ActivitiesMessageBoards = Activities.ActivitiesMessageBoards.Select(m => new GetActMsgBoardDto
                {
                    ActivitiesMessageId = m.ActivitiesMessageId,
                    ActivitiesMessageContent = m.ActivitiesMessageContent,
                    ActivitiesCreatedDate = m.ActivitiesCreatedDate,
                    Account = m.User.Account
                })
            };
            return Ok(res);
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
            if (UserID != id)
            {
                return BadRequest("權限不符");
            }
            if (user == null)
            {
                return BadRequest("已退出此活動");
            }
            var quit = _context.JoinMembers.Where(j => j.Member.UserId == UserID).SingleOrDefault();
            _context.JoinMembers.Remove(quit);
            var act = _context.SocialActivities.Include(s => s.JoinMembers).Where(s => s.ActivityId == id).SingleOrDefault();
            if (act == null)
            {
                return BadRequest("無此活動");
            }
            act.JoinCount = _context.JoinMembers.Where(j => j.ActivitiesId == id).Count();
            _context.SocialActivities.Update(act);
            _context.SaveChanges();
            return Ok("您已退出此活動");
        }
        //活動刪除
        [HttpDelete("DeleteAct/{id}")]
        public IActionResult DeleteAct(int id)
        {
            var UserID = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var Act = _context.SocialActivities.Include(s=>s.JoinMembers).Include(s=>s.ActivitiesMessageBoards).SingleOrDefault(s=>s.ActivityId == id);
            if (UserID != id)
            {
                return BadRequest("權限不符");
            }
            if (Act == null)
            {
                return BadRequest("無此活動");
            }
            _context.SocialActivities.Remove(Act);
            _context.SaveChanges();
            return Ok("您已刪除此活動");
        }
    }
}
