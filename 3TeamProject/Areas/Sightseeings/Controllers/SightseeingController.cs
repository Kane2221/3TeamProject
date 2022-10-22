using _3TeamProject.Areas.Sightseeings.Models;
using _3TeamProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var Sight = from s in _context.Sightseeings
                        join p in _context.SightseeingPictureInfos
                        on s.SightseeingId equals p.SightseeingId
                        select new SightGetViewModel
                        {
                            SightseeingId = s.SightseeingId,
                            SightseeingName = s.SightseeingName,
                            SightseeingCountry = s.SightseeingCountry,
                            SightseeingCity = s.SightseeingCity,
                            SightseeingAddress = s.SightseeingAddress,
                            SightseeingScore = s.SightseeingScore,
                            SightseeingPictureId = p.SightseeingPictureId,
                            SightseeingPictureName = p.SightseeingPictureName,
                            SightseeingPicturePath = p.SightseeingPicturePath,
                        };
            return Ok(Sight);
        }
        [HttpGet("{id}")]
        public IActionResult GetSightseeingsDetail(int id) //TODO 景點詳細頁
        {
            var Sight = (from s in _context.Sightseeings
                        join p in _context.SightseeingPictureInfos
                        on s.SightseeingId equals p.SightseeingId
                        join b in _context.SightseeingMessageBoards
                        on s.SightseeingId equals b.SightseeingId
                        where s.SightseeingId == id
                        select new SightGetDetailViewModel
                        {
                            SightseeingId = s.SightseeingId,
                            SightseeingName = s.SightseeingName,
                            SightseeingCountry = s.SightseeingCountry,
                            SightseeingCity = s.SightseeingCity,
                            SightseeingAddress = s.SightseeingAddress,
                            SightseeingScore = s.SightseeingScore,
                            SightseeingPictureId = p.SightseeingPictureId,
                            SightseeingPictureName = p.SightseeingPictureName,
                            SightseeingPicturePath = p.SightseeingPicturePath,
                            SightseeingIntroduce = s.SightseeingIntroduce,
                            MessageBoardId = b.MessageBoardId,
                            SightseeingMessageContent = b.SightseeingMessageContent,
                            MessageCreatedTime = b.MessageCreatedTime,
                        }).SingleOrDefault();

            return Ok(Sight);
        }
    }
}
