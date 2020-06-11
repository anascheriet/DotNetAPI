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
    public class PublicationController : ControllerBase
    {
        private readonly IGestionRepository _repo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public PublicationController(IGestionRepository repo, UserManager<AppUser> userManager, IMapper mapper)
        {
            _repo = repo;
            _userManager = userManager;
            _mapper = mapper;
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddPublication(PublicationAddDto publicationAddDto)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var pub = _mapper.Map<Publication>(publicationAddDto);
            pub.Owner = user;
            _repo.Add(pub);
            if (publicationAddDto.Files != null)
            {
                //Save the publication to get the new id
                if (!await _repo.Save()) return BadRequest("ERROR, Problem occured while adding the publication");

                foreach (string filepath in publicationAddDto.Files)
                {
                    _repo.Add(new Attachment
                    {
                        PublicationId = pub.PublicationId,
                        path = filepath
                    });
                }
            }

            if (await _repo.Save()) return Ok("Publication added successfully !");
            return BadRequest("ERROR, Problem occured while adding the publication");

        }

        [HttpPost("{pubid}")]
        public async Task<IActionResult> EditPublication(int pubid, PublicationAddDto publicationAddDto)
        {
            var pub = await _repo.GetPublication(pubid);
            _mapper.Map(publicationAddDto, pub);


            if (await _repo.Save()) return Ok("Publication Modified successfully !");
            return BadRequest("ERROR, Problem occured while modifying the publication");

        }
        [HttpPost("{pubid}/delete")]
        public async Task<IActionResult> DeletePublication(int pubid)
        {
            var pub = await _repo.GetPublication(pubid);
            _repo.Delete(pub);

            if (await _repo.Save()) return Ok("Publication Deleted successfully !");
            return BadRequest("ERROR, Problem occured while deleting the publication");

        }

        [HttpGet("ofclass/{classid}")]
        public async Task<IActionResult> GetPublications(int classid)
        {
            var Publications = await _repo.GetPublications(classid);
            var publicationsToReturn = _mapper.Map<IEnumerable<PublicationForListDto>>(Publications);
            return Ok(publicationsToReturn);
        }

        [HttpGet("{pubid}")]
        public async Task<IActionResult> GetPublication(int pubid)
        {
            var pub = await _repo.GetPublication(pubid);
            var pubToReturn = _mapper.Map<PublicationForListDto>(pub);
            return Ok(pubToReturn);
        }
        [HttpGet("{pubid}/Attachment")]
        public async Task<IActionResult> GetAttachments(int pubid)
        {
            var atts = await _repo.GetAttachments(pubid);
            var attachmentsToReturn = _mapper.Map<IEnumerable<AttachmentForListDto>>(atts);
            return Ok(attachmentsToReturn);
        }
        [HttpGet("Attachment/{attid}")]
        public async Task<IActionResult> GetAttachment(int attid)
        {
            var att = await _repo.GetAttachment(attid);
            var attachmentToReturn = _mapper.Map<AttachmentForListDto>(att);
            return Ok(attachmentToReturn);
        }


    }
}