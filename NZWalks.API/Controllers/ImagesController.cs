using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepositpry _imageRepositpry;

        public ImagesController(IImageRepositpry imageRepositpry)
        {
            _imageRepositpry = imageRepositpry;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadDTO imageDTO)
        {
            ValidateUploadedFile(imageDTO);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var image = new Image
            {
                FileName = imageDTO.FileName,
                File = imageDTO.File,
                Description = imageDTO.Description,
                FileExtention = Path.GetExtension(imageDTO.File.FileName),
                FileSizeInBytes = imageDTO.File.Length
            };

            await _imageRepositpry.Upload(image);

            return Ok(image);
        }

        private void ValidateUploadedFile(ImageUploadDTO imageDTO)
        {
            var acceptableExtentions = new string[] { ".png", ".jpeg", ".jpg" };

            if (!acceptableExtentions.Contains(Path.GetExtension(imageDTO.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsuported file extention");
            }

            if (imageDTO.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "file size is more than 10MB");
            }
                
        }
    }
}
