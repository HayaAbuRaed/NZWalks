using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepo;

        public WalksController(IMapper mapper, IWalkRepository walkRepo)
        {
            _mapper = mapper;
            _walkRepo = walkRepo;
        }

        // GetAllWalks:
        // api/Walks?filterOn=Name&filterQuery=Track&sortOn=Name&isAscending=true
        [HttpGet]
        public async Task<IActionResult> GetWalks([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortOn, [FromQuery] bool? isAssending, [FromQuery] int? pageSize, [FromQuery] int? pageNo)
        {
            var walks = await _walkRepo.GetWalksAsync(filterOn, filterQuery, sortOn, isAssending ?? true, pageSize ?? 1, pageNo ?? 1000);

            return Ok(_mapper.Map<List<WalkDTO>>(walks));
        }

        // Get Walk by id:
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWalkById([FromRoute] Guid id)
        {
            var walk = await _walkRepo.GetWalkByIdAsync(id);

            if (walk == null) return NotFound($"There is no walk with id = {id}");

            return Ok(_mapper.Map<WalkDTO>(walk));
        }

        // Create a Walk:
        [HttpPost]
        public async Task<IActionResult> CreateWalk([FromBody] AddWalkDTO addWalkDto)
        {
            var walk = _mapper.Map<Walk>(addWalkDto);

            await _walkRepo.AddWalkAsync(walk);

            return CreatedAtAction(nameof(GetWalkById), new { id = walk.Id }, _mapper.Map<WalkDTO>(walk));
        }

        [HttpPut("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid id, [FromBody] UpdateWalkDTO updateWalkDTO)
        {
            var walk = _mapper.Map<Walk>(updateWalkDTO);

            walk = await _walkRepo.UpdateWalkAsync(id, walk);

            if (walk == null)
                return NotFound($"There is no walk with id = {id}");

            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteWalk([FromRoute] Guid id)
        {
            var walk = await _walkRepo.DeleteWalkAsync(id);

            if (walk == null) return NotFound();

            return Ok("Walk is deleted");
        }

    }
}
