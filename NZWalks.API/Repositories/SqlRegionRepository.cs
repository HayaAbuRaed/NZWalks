using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SqlRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _dbContext;

        public SqlRegionRepository(NZWalksDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await _dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetRegionAsync(Guid id)
        {
            return await _dbContext.Regions.FindAsync(id);
        }

        public async Task DeleteRegionAsync(Region region)
        {
            // there is no Async remove method 
            _dbContext.Regions.Remove(region);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Region?> CreateRegionAsync(Region region)
        {
            await _dbContext.Regions.AddAsync(region);
            await _dbContext.SaveChangesAsync();
            return region;
        }

        public async Task UpdateRegion()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
