using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Linq;

namespace NZWalks.API.Repositories
{
    public class SqlWalkRepository : IWalkRepository
    {
        private NZWalksDbContext _dbContext;

        public SqlWalkRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Walk> AddWalkAsync(Walk walk)
        {
            await _dbContext.Walks.AddAsync(walk);
            _dbContext.SaveChanges();
            return walk;
        }

        public async Task<Walk?> GetWalkByIdAsync(Guid id)
        {
            return await _dbContext.Walks.Include("Region").Include("Difficulty").FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<List<Walk>> GetWalksAsync(string? filterOn = null, string? filterQuery = null, string? sortOn = null, bool isAssending = true, int pageSize = 1, int pageNo = 1000)
        {
            var walks = _dbContext.Walks.Include("Region").Include("Difficulty").AsQueryable();

            // Filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(w => w.Name.Contains(filterQuery));
                }
            }

            // Sorting
            if (string.IsNullOrWhiteSpace(sortOn) == false)
            {
                if (sortOn.Equals("name", StringComparison.OrdinalIgnoreCase))
                    walks = isAssending ? walks.OrderBy(w => w.Name) : walks.OrderByDescending(w => w.Name);

                else if (sortOn.Equals("length", StringComparison.OrdinalIgnoreCase))
                    walks = isAssending ? walks.OrderBy(w => w.LengthInKM) : walks.OrderByDescending(w => w.LengthInKM);
            }

            //pagination
            var skipBy = (pageNo - 1) * pageSize;

            return await walks.Skip(skipBy).Take(pageSize).ToListAsync();
        }

        public async Task<Walk?> UpdateWalkAsync(Guid id, Walk walk)
        {
            var existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(w => w.Id == id);

            if (existingWalk == null) 
                return null;

            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKM = walk.LengthInKM;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.DifficultyId = walk.DifficultyId;

            await _dbContext.SaveChangesAsync();

            return existingWalk;
        }

        public async Task<Walk?> DeleteWalkAsync(Guid id)
        {
            var walk = await _dbContext.Walks.FindAsync(id);

            if (walk == null) return null;

            _dbContext.Walks.Remove(walk);
            _dbContext.SaveChanges();

            return walk;
        }
    }
}
