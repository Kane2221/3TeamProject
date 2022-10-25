﻿using _3TeamProject.Areas.Sightseeings.Data;
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
    public class SightseeingController : ControllerBase
    {
        private readonly _3TeamProjectContext _context;

        public SightseeingController(_3TeamProjectContext context)
        {
            _context=context;
        }
        //景點所有清單
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
        //景點詳細資料
        [HttpGet("{id}")]
        public IActionResult GetSightseeingsDetail(int id)
        {
            var UserRole = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            var Sight = _context.Sightseeings.Include(p => p.SightseeingPictureInfos)
                .Include(m => m.SightseeingMessageBoards).Include(c => c.SightseeingCategory)
                .Where(s => s.SightseeingId == id).Select(s => new SightGetDetailViewModel
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
                        Account = m.User.Account,
                        SightseeingMessageContent = m.SightseeingMessageContent,
                        MessageCreatedTime = m.MessageCreatedTime,
                        RoleName = m.User.RolesNavigation.RoleName
                    })
                }).FirstOrDefault();
            return Ok(Sight);
        }
        //景點留言
        [Authorize]
        [HttpPost]
        public IActionResult PostMessage(SightPostMsgViewModel request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                return BadRequest(errors);
            }
            var UserId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value);
            var message = new SightseeingMessageBoard
            {
                SightseeingId = request.SightseeingId,
                UserId = UserId,
                SightseeingMessageContent = request.SightseeingMessageContent,
                MessageCreatedTime = DateTime.Now,
                SightseeingStatusId =1
            };
            _context.SightseeingMessageBoards.Add(message);
            _context.SaveChanges();
            var response = _context.Sightseeings.Include(p => p.SightseeingPictureInfos)
                .Include(m => m.SightseeingMessageBoards).Include(c => c.SightseeingCategory)
                .Where(s => s.SightseeingId == request.SightseeingId).Select(s => new SightGetDetailViewModel
                {
                    SightseeingId = s.SightseeingId,
                    SightseeingMessageBoards = s.SightseeingMessageBoards.Select(m => new SightMessageViewModel
                    {
                        MessageBoardId = m.MessageBoardId,
                        Account = m.User.Account,
                        SightseeingMessageContent = m.SightseeingMessageContent,
                        MessageCreatedTime = m.MessageCreatedTime,
                        RoleName = m.User.RolesNavigation.RoleName
                    })
                });
            return Ok(response);
        }

                
            
        
    }
}
