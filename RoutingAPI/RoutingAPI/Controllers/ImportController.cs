using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RoutingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<String>> CheckConn()
        {
            return "Connected to Routing API : Import Controller";
        }

        [HttpPost("zip")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                var folderName = Path.Combine("Resources", "uploads");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    string zipPath = Path.GetFileName(fullPath);
                    ZipFile.ExtractToDirectory(fullPath, pathToSave);

                    return Ok();
                }
                else
                {
                    return BadRequest("Error in uploading file");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
