using Microsoft.AspNetCore.Mvc;
using sheargenius_backend.Services;

namespace BackendBlog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private readonly BlobService _blobService;

        public BlobController(BlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpPost("Upload")]
        //We Will create an Async Function That takes in a IFormFile and the file name
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string fileName)
        {
        //If our File is null return invalid
            if (file == null || file.Length == 0) return BadRequest("Invalid file.");
						
						//Simplify our using statement to dispose of the file after we stream it
            using var stream = file.OpenReadStream();
            //Pass our stream and our file name into the Method to get the url
            var fileUrl = await _blobService.UploadFileAsync(stream, fileName); // Use filename from frontend
						
						//Return the URL to the front end VIA OK Method
            return Ok(new { FileUrl = fileUrl });
        }

    }
}