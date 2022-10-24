using _3TeamProject.Areas.Sightseeings.Data;
using _3TeamProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace _3TeamProject.Areas.Sightseeings.Controllers
{
    [Authorize(Roles = ("Administrator, ChiefAdministrator, SuperAdministrator"))]
    [Route("Sightseeings/[controller]")]
    [ApiController]
    public class SightseeingController : ControllerBase
    {
        private readonly _3TeamProjectContext _context;

        public SightseeingController(_3TeamProjectContext context)
        {
            _context=context;
        }
        [HttpGet]
        public IActionResult GetAllSightseeings()
        {
            var Sight = _context.Sightseeings.Include(s => s.SightseeingPictureInfos)
                .Include(s => s.SightseeingCategory).Select(s => new SightGetViewModel
                {
                    SightseeingId = s.SightseeingId,
                    SightseeingName = s.SightseeingName,
                    SightseeingCountry = s.SightseeingCountry,
                    SightseeingCity = s.SightseeingCity,
                    SightseeingAddress = s.SightseeingAddress,
                    SightseeingScore = s.SightseeingScore,
                    CategoryName = s.SightseeingCategory.CategoryName,
                    SightseeingHomePage = s.SightseeingHomePage,
                    SightseeingPictureInfos = s.SightseeingPictureInfos.Select(p => new SightPicInfoViewModel
                    {
                        SightseeingPictureId = p.SightseeingPictureId,
                        SightseeingPictureName = p.SightseeingPictureName,
                        SightseeingPicturePath = p.SightseeingPicturePath
                    })
                });

            return Ok(Sight);
        }
        [HttpGet("{id}")]
        public IActionResult GetSightseeingsDetail(int id)
        {
            var Sight = _context.Sightseeings.Include(p=> p.SightseeingPictureInfos)
                .Include(m => m.SightseeingMessageBoards).Include(c => c.SightseeingCategory)
                .Include(e=>e.SightseeingMessageBoards)
                .Where(s => s.SightseeingId == id).Select(s=>new SightGetDetailViewModel
                {
                    SightseeingId = s.SightseeingId,
                    SightseeingName = s.SightseeingName,
                    SightseeingCountry = s.SightseeingCountry,
                    SightseeingCity = s.SightseeingCity,
                    SightseeingAddress = s.SightseeingAddress,
                    SightseeingScore = s.SightseeingScore,
                    CategoryName = s.SightseeingCategory.CategoryName,
                    SightseeingPictureInfos = s.SightseeingPictureInfos.Select(p => new SightPicInfoViewModel
                    {
                        SightseeingPictureId = p.SightseeingPictureId,
                        SightseeingPictureName = p.SightseeingPictureName,
                        SightseeingPicturePath = p.SightseeingPicturePath
                    }),
                    SightseeingMessageBoards = s.SightseeingMessageBoards.Select(m => new SightMessageViewModel
                    {
                        MessageBoardId = m.MessageBoardId,
                        MemberName = m.Member.MemberName,
                        SightseeingMessageContent = m.SightseeingMessageContent,
                        MessageCreatedTime = m.MessageCreatedTime
                    })
                }).FirstOrDefault();
            
            return Ok(Sight);
        }
    }
}
