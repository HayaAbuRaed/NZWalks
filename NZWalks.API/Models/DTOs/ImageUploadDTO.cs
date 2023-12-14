using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTOs
{
    public class ImageUploadDTO
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        public string FileName { get; set; }

        public string? Description { get; set; }
    }
}
