using _3TeamProject.Areas.Sightseeings.Data;
using _3TeamProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace _3TeamProject.Areas.Sightseeings.Controllers
{
    [Route("Sightseeings/[controller]")]
    [ApiController]
    public class SightseeingApiController : ControllerBase
    {
        private readonly _3TeamProjectContext _context;

        public SightseeingApiController(_3TeamProjectContext context)
        {
            _context=context;
        }
        //景點所有清單
        [HttpGet]
        public IActionResult GetAllSightseeings()
        {
            var Sight = _context.Sightseeings.Include(s => s.SightseeingPictureInfos)
                .Include(s => s.SightseeingCategory).Select(s => new GetSightDto
                {
                    SightseeingId = s.SightseeingId,
                    SightseeingName = s.SightseeingName,
                    SightseeingCountry = s.SightseeingCountry,
                    SightseeingCity = s.SightseeingCity,
                    SightseeingAddress = s.SightseeingAddress,
                    SightseeingScore = s.SightseeingScore,
                    CategoryName = s.SightseeingCategory.CategoryName,
                    SightseeingHomePage = s.SightseeingHomePage,
                    SightseeingPictureInfos = s.SightseeingPictureInfos.Select(p => new GetSightPicInfoDto
                    {
                        SightseeingPictureId = p.SightseeingPictureId,
                        SightseeingPictureName = p.SightseeingPictureName,
                        SightseeingPicturePath = p.SightseeingPicturePath
                    })
                });

            return Ok(Sight);
        }
        //景點詳細資料
        [HttpGet("{id}")]
        public IActionResult GetSightseeingsDetail(int id)
        {
            var Sight = _context.Sightseeings.Include(p => p.SightseeingPictureInfos)
                .Include(m => m.SightseeingMessageBoards).Include(c => c.SightseeingCategory)
                .Where(s => s.SightseeingId == id).Select(s => new GetSightDetailDto
                {
                    SightseeingId = s.SightseeingId,
                    SightseeingName = s.SightseeingName,
                    SightseeingCountry = s.SightseeingCountry,
                    SightseeingCity = s.SightseeingCity,
                    SightseeingAddress = s.SightseeingAddress,
                    SightseeingScore = s.SightseeingScore,
                    SightseeingIntroduce = s.SightseeingIntroduce,
                    CategoryName = s.SightseeingCategory.CategoryName,
                    SightseeingPictureInfos = s.SightseeingPictureInfos.Select(p => new GetSightPicInfoDto
                    {
                        SightseeingPictureId = p.SightseeingPictureId,
                        SightseeingPictureName = p.SightseeingPictureName,
                        SightseeingPicturePath = p.SightseeingPicturePath
                    }),
                    SightseeingMessageBoards = s.SightseeingMessageBoards.Select(m => new SightMessageDto
                    {
                        MessageBoardId = m.MessageBoardId,
                        Account = m.User.Account,
                        SightseeingMessageContent = m.SightseeingMessageContent,
                        MessageCreatedTime = m.MessageCreatedTime.ToShortDateString(),
                        RoleName = m.User.RolesNavigation.RoleName
                    })
                }).SingleOrDefault(); ;
            if (Sight == null)
            {
                return NotFound("沒有此景點");
            }
            return Ok(Sight);


        }
        //景點留言
        [Authorize]
        [HttpPost]
        public IActionResult PostMessage([FromBody] PostSightMsgDto request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }
            var UserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value;
            if (UserId == null)
            {
                return NotFound("帳號未登入");
            }
            string UserrRole = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            var message = new SightseeingMessageBoard
            {
                SightseeingId = request.SightseeingId,
                UserId = int.Parse(UserId),
                SightseeingMessageContent = request.SightseeingMessageContent,
                MessageCreatedTime = DateTime.Now,
                SightseeingStatusId =1
            };
            _context.SightseeingMessageBoards.Add(message);
            _context.SaveChanges();

            return Ok("留言成功");
        }

                
            
        
    }
}
