using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.Dto;
using DotnetAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly IGestionRepository _repo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public CommentController(IGestionRepository repo, UserManager<AppUser> userManager, IMapper mapper)
        {
            _repo = repo;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddComment(CommentForAddDto commentForAddDto)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var comment = _mapper.Map<Comment>(commentForAddDto);
            comment.Owner = user;
            _repo.Add(comment);

            if (await _repo.Save()) return Ok("Comment added successfully !");
            throw new Exception("ERROR, Problem occured while adding the Comment");

        }

        [HttpGet("publication{pubid}")]
        public async Task<IActionResult> GetComments(int pubid)
        {
            var comments = await _repo.GetComments(pubid);
            var commentsToReturn = _mapper.Map<IEnumerable<CommentForListDto>>(comments);
            return Ok(commentsToReturn);
        }
        [HttpGet("{comid}")]
        public async Task<IActionResult> GetComment(int comid)
        {
            var comment = await _repo.GetComment(comid);
            var commentToReturn = _mapper.Map<CommentForListDto>(comment);
            return Ok(commentToReturn);
        }

        [HttpPost]
        public async Task<IActionResult> EditComment(CommentForEditDto commentForAddDto)
        {
            var com = await _repo.GetComment(commentForAddDto.CommentId);
            _mapper.Map(commentForAddDto, com);


            if (await _repo.Save()) return Ok("Comment Modified successfully !");
            throw new Exception("ERROR, Problem occured while modifying the Comment");

        }
        [HttpPost("{comid}/delete")]
        public async Task<IActionResult> DeletePublication(int comid)
        {
            var com = await _repo.GetComment(comid);
            _repo.Delete(com);

            if (await _repo.Save()) return Ok("Comment Deleted successfully !");
            throw new Exception("ERROR, Problem occured while deleting the Comment");

        }
    }
}