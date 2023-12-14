using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IImageRepositpry
    {
        Task<Image> Upload(Image image);
    }
}
