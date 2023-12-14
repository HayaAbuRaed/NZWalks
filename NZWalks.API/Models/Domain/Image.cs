using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalks.API.Models.Domain
{
    public class Image
    {
        public Guid Id { get; set; }

        [NotMapped] // denotes that the property should be excluded from database mapping
        public IFormFile File { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string? Description { get; set; }
        public string FileExtention { get; set; }
        public long FileSizeInBytes { get; set; }
    }
}
