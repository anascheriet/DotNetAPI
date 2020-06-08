using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public class ClassController : ControllerBase
    {
        private readonly IGestionRepository _repo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public ClassController(IGestionRepository repo,UserManager<AppUser> userManager, IMapper mapper)
        {
            _repo = repo;
            _userManager = userManager;
            _mapper = mapper;
        }
        

        [HttpGet]
        public async Task<IActionResult> getClasses()
        {
            //get current logged in user
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var classes = await _repo.GetClasses(user.Id);
            var classesToReturn = _mapper.Map<List<ClassForListDto>>(classes);
            return Ok(classesToReturn);
        }
        
        

        [HttpGet("{id}")]
        public async Task<IActionResult> getClass(int id)
        {
            //get current logged in user
            var classe = await _repo.GetClass(id);
            var classToReturn = _mapper.Map<ClassForListDto>(classe);
            return Ok(classToReturn);
        }
        [HttpGet("{id}/members")]
        public async Task<IActionResult> getClassMembers(int id)
        {
            
            var members = await _repo.GetClassMembers(id);
            var membersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(members);
            return Ok(membersToReturn);
        }

        [HttpPost("{classid}/transfer/{userid}")]
        public async Task<IActionResult> TransferOwnership(int classid,int userid)
        {
            
            var classe = await _repo.GetClass(classid);
            var user = await _repo.GetUser(userid);

            classe.Owner = user;
            await _repo.Save();
            
            return Ok("Transfered successfully !");
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AjouterClasse(ClassForCrudDto classForCrudDto) /// IL faut créer un DTO 
        {
            var classe =  _mapper.Map<Class>(classForCrudDto);
            classe.Owner =  _userManager.GetUserAsync(HttpContext.User).Result;
            _repo.Add(classe);
            var classmember = new ClassAppUser {
                Class = classe,
                AppUser = classe.Owner,
                verified = true
            };
            _repo.Add(classmember);


            if(await _repo.Save()) return Ok("Class added successfully !");
            return BadRequest("ERROR, Problem occured while adding you to the class members");
        }

        [HttpPost("Join/{code}")]
        public async Task<IActionResult> JoinClass(string code)
        {
            //get current logged in user
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var classe = _repo.GetClassByCode(code).Result;

            _repo.Add(new ClassAppUser{
                AppUser = user,
                Class = classe
            });

            if(await _repo.Save()) return Ok("Joining Request sent, please wait for your teacher to accept !");
            return BadRequest("ERROR, Problem occured while adding you to the class members");
        }

        [HttpPost("{classid}/kick/{userid}")]
        public async Task<IActionResult> kickClassMember(int userid, int classid)
        {
            _repo.Delete(await _repo.GetClassMemberRelation(userid,classid));

            if(await _repo.Save()) return Ok("Member kicked from your class successfully !");
            return BadRequest("ERROR, Problem occured while kicking member from your class");
        }

        [HttpPost("{classid}/delete")]
        public async Task<IActionResult> DeleteClass(int classid)
        {
            _repo.Delete(await _repo.GetClass(classid));

            if(await _repo.Save()) return Ok("Class deleted successfully !");
            return BadRequest("ERROR, Problem occured while deleting the class");
        }

        [HttpPost("{classid}")]
        public async Task<IActionResult> Edit(int classid, ClassForCrudDto classForCrudDto)
        {
            var classe = await _repo.GetClass(classid);
            _mapper.Map(classForCrudDto,classe);
            
            if(await _repo.Save()) return Ok("Class Modified successfully !");
            return BadRequest("ERROR, Problem occured while modifying the class");
        }
        
    }
}