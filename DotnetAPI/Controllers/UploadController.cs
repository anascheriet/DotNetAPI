using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DotnetAPI.Data;
using DotnetAPI.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [Route("[controller]")]
    public class UploadController : ControllerBase
    {

        private readonly IWebHostEnvironment _env;
        private readonly IGestionRepository _repo;

        public UploadController(IWebHostEnvironment env, IGestionRepository repo)
        {
            _env = env;
            _repo = repo;

        }


        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Uploads");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length == 0)
                {
                    throw new Exception("Error, no file");
                }
                else
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(dbPath);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [Authorize]
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] attachmentdata att)
        {

            var at = await _repo.GetAttachment(att.fileid);
            _repo.Delete(at);

            if (await _repo.Save())
            {
                string filename = _env.ContentRootPath + "/" + att.filepath;
                System.IO.File.Delete(filename);
                return Ok("File Deleted Successfully");
            }
            throw new Exception("ERROR, Problem occured while deleting the attachment");



        }
    }
    public class attachmentdata
    {
        public int fileid { get; set; }
        public string filepath { get; set; }

    }
}