using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?> GetRegionAsync(Guid id);
        Task<Region?> CreateRegionAsync(Region region);
        Task DeleteRegionAsync(Region region);
        Task UpdateRegion();
    }
}
