using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        public ClassController(IGestionRepository repo, UserManager<AppUser> userManager, IMapper mapper)
        {
            _repo = repo;
            _userManager = userManager;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> getClasses()
        {
            //get current logged in user
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByIdAsync(userid);
            var classes = await _repo.GetClasses(user.Id);
            var classesToReturn = _mapper.Map<List<ClassForListDto>>(classes);

            foreach (var classe in classesToReturn)
            {

                var verifiedMembers = await _repo.GetClassMembers(classe.ClassId);
                var pendingMembers = await _repo.GetPendingMembers(classe.ClassId);
                classe.Members = _mapper.Map<List<UserForListDto>>(verifiedMembers);
                classe.Pending = _mapper.Map<List<UserForListDto>>(pendingMembers);
            }
            return Ok(classesToReturn);

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> getClass(int id)
        {
            //get current logged in user
            var classe = await _repo.GetClass(id);
            var classToReturn = _mapper.Map<ClassForListDto>(classe);

            var verifiedMembers = await _repo.GetClassMembers(classe.ClassId);
            var pendingMembers = await _repo.GetPendingMembers(classe.ClassId);
            classToReturn.Members = _mapper.Map<List<UserForListDto>>(verifiedMembers);
            classToReturn.Pending = _mapper.Map<List<UserForListDto>>(pendingMembers);

            return Ok(classToReturn);
        }


        [HttpPost("{classid}/transfer/{userid}")]
        public async Task<IActionResult> TransferOwnership(int classid, int userid)
        {

            var classe = await _repo.GetClass(classid);
            var user = await _repo.GetUser(userid);

            classe.Owner = user;
            await _repo.Save();

            return Ok("Transfered successfully !");
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AjouterClasse(ClassForCrudDto classForCrudDto)
        {
            var classe = _mapper.Map<Class>(classForCrudDto);
            classe.Owner = _userManager.GetUserAsync(HttpContext.User).Result;
            _repo.Add(classe);
            var classmember = new ClassAppUser
            {
                Class = classe,
                Member = classe.Owner,
                verified = true
            };
            _repo.Add(classmember);


            if (await _repo.Save()) return Ok("Class added successfully !");
            throw new Exception("ERROR, Problem occured while adding you to the class members");
        }

        [HttpPost("Join/{code}")]
        public async Task<IActionResult> JoinClass(string code)
        {
            //get current logged in user
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var classe = await _repo.GetClassByCode(code);

            var relation = await _repo.GetClassMemberRelation(user.Id, classe.ClassId);
            if (relation != null) throw new Exception("Joining Request already sent, please wait for your teacher to accept !");


            _repo.Add(new ClassAppUser
            {
                MemberId = user.Id,
                ClassId = classe.ClassId
            });

            if (await _repo.Save()) return Ok("Joining Request sent, please wait for your teacher to accept !");
            throw new Exception("ERROR, Problem occured while adding you to the class members");
        }

        [HttpPost("{classid}/kick/{userid}")]
        public async Task<IActionResult> kickClassMember(int userid, int classid)
        {
            _repo.Delete(await _repo.GetClassMemberRelation(userid, classid));

            if (await _repo.Save()) return Ok("Member kicked from your class successfully !");
            throw new Exception("ERROR, Problem occured while kicking member from your class");
        }

        public class tempResponse
        {
            public bool resp { get; set; }
        }
        [HttpPost("{classid}/handle/{userid}")]
        public async Task<IActionResult> handleClassMember([FromBody] tempResponse decision, int userid, int classid)
        {
            if (decision.resp)
            {
                var relation = await _repo.GetClassMemberRelation(userid, classid);
                relation.verified = true;
                if (await _repo.Save()) return Ok("Member invitation accepted successfully !");
                throw new Exception("ERROR, Problem occured while accepting the invite");

            }
            else
            {
                _repo.Delete(await _repo.GetClassMemberRelation(userid, classid));
                if (await _repo.Save()) return Ok("Member removed from class !");
                throw new Exception("ERROR, Problem occured while declining the invite");

            }

        }

        [HttpPost("{classid}/delete")]
        public async Task<IActionResult> DeleteClass(int classid)
        {
            _repo.Delete(await _repo.GetClass(classid));

            if (await _repo.Save()) return Ok("Class deleted successfully !");
            throw new Exception("ERROR, Problem occured while deleting the class");
        }

        [HttpPost("{classid}")]
        public async Task<IActionResult> Edit(int classid, ClassForCrudDto classForCrudDto)
        {
            var classe = await _repo.GetClass(classid);
            _mapper.Map(classForCrudDto, classe);

            if (await _repo.Save()) return Ok("Class Modified successfully !");
            throw new Exception("ERROR, Problem occured while modifying the class");
        }

    }
}