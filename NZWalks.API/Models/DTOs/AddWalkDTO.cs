using NZWalks.API.Models.Domain;

namespace NZWalks.API.Models.DTOs
{
    public class AddWalkDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int LengthInKM { get; set; }
        public string? WalkImageUrl { get; set; }
        public Guid RegionId { get; set; }
        public Guid DifficultyId { get; set; }
        
    }
}
