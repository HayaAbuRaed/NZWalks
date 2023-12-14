using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(IRegionRepository regionRepo, IMapper mapper, ILogger<RegionsController> logger)
        {
            _regionRepo = regionRepo;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetStudents()
        {
            _logger.LogInformation("Hi");
            try
            {
                throw new Exception("this is a custom exception");
                // get region domain model from the database
                var regions = await _regionRepo.GetAllAsync();

                // map it to DTO object & return the Dto instead or the domain model obj
                return Ok(_mapper.Map<List<RegionDTO>>(regions));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetStudentById([FromRoute]Guid id)
        {
            // var region = _dbContext.Regions.FirstOrDefault(r => r.Id == id);

            // the Find method only works with the primary key
            var region = await _regionRepo.GetRegionAsync(id);

            if (region is null)
                return NotFound($"Region with id {id} does not exist.");

            return Ok(_mapper.Map<RegionDTO>(region));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> PostRegion([FromBody] AddRegionDTO addRegionDTO)
        {
            var region = _mapper.Map<Region>(addRegionDTO);

            await _regionRepo.CreateRegionAsync(region);

            var regionDTO = _mapper.Map<RegionDTO>(region);

            return CreatedAtAction(nameof(GetStudentById), new { id = regionDTO.Id }, regionDTO);
        }

        [HttpPut("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id,[FromBody] UpdateRegionDTO updateRegionDTO)
        {
            var region = await _regionRepo.GetRegionAsync(id);

            if (region is null)
                return NotFound($"No region with id = {id}");

            region.Name = updateRegionDTO.Name;
            region.Code = updateRegionDTO.Code;
            region.RegionImageURL = updateRegionDTO.RegionImageURL;

            await _regionRepo.UpdateRegion();

            return NoContent();
        }

        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id)
        {
            var region = await _regionRepo.GetRegionAsync(id);

            if (region is null)
                return NotFound($"No region with id = {id}");

            await _regionRepo.DeleteRegionAsync(region);

            return Ok();
        }

    }
}
