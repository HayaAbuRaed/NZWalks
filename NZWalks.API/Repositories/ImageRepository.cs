using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class ImageRepository : IImageRepositpry
    {
        // provides information about the web host environment. 
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _accessor;
        private readonly NZWalksDbContext _dbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor accessor, NZWalksDbContext dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _accessor = accessor;
            _dbContext = dbContext;
        }
        public async Task<Image> Upload(Image image)
        {
            
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", $"{image.File.FileName}{image.FileExtention}");

            // UPLOAD IMAGE TO LOCALPATH
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            //                                  http                  ://               localhost 
            var urlFilePath = $"{_accessor.HttpContext.Request.Scheme}://{_accessor.HttpContext.Request.Host}{_accessor.HttpContext.Request.PathBase}/Images/{image.File.FileName}{image.FileExtention}";
            //               :1786                   /Images/       imagexyz              .abc 

            image.FilePath = urlFilePath;

            await _dbContext.Images.AddAsync(image);
            await _dbContext.SaveChangesAsync();

            return image;
        }
    }
}

// _webHostEnvironment.ContentRootPath => (the root directory of the web application)
// Path.Combine() => By using it, we ensure that the parts of the path are joined together using the correct path separator for the current operating system.


// FileStream instance used to write data to a file on the local file system. 
// FileMode.Create => pecifies that the file should be created if it doesn't exist or overwritten if it does. If the file already exists, its contents will be cleared.