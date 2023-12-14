using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> AddWalkAsync(Walk walk);
        Task<Walk?> GetWalkByIdAsync(Guid id);
        Task<List<Walk>> GetWalksAsync(string? filterOn = null, string? filterQuery = null, string? sortOn = null, bool isAssending = true, int pageSize = 1, int pageNo = 1000);
        Task<Walk?> UpdateWalkAsync(Guid id, Walk walk);
        Task<Walk?> DeleteWalkAsync(Guid id);
    }
}
