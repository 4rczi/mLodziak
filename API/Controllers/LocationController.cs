using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedServices;
using SharedModels;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("{userId}/{categoryId}")]
        [Authorize(Policy = "AccessTokenPolicy")]
        public async Task<ActionResult<List<LocationModel>>> GetLocations([FromRoute] string userId, [FromRoute] int categoryId)
        {
            var locationModels = await _locationService.GetLocationModelsAsync(userId, categoryId);

            if (locationModels.IsNullOrEmpty())
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(locationModels);
        }

        [HttpGet("{userId}/all")]
        [Authorize(Policy = "AccessTokenPolicy")]
        public async Task<ActionResult<Dictionary<int, List<LocationModel>>>> GetAllLocations([FromRoute] string userId)
        {
            var locationModels = await _locationService.GetAllLocationModelsAsync(userId);

            if (locationModels.IsNullOrEmpty())
            {
                return NotFound();
            }

            return Ok(locationModels);
        }

        [HttpGet("{userId}/single/{physicalLocationId}")]
        [Authorize(Policy = "AccessTokenPolicy")]
        public async Task<ActionResult<LocationModel>> GetSingleLocation([FromRoute] string userId, [FromRoute] int physicalLocationId)
        {
            var locationModel = await _locationService.GetLocationModelAsync(physicalLocationId, userId);

            if (locationModel == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(locationModel);
        }
    }
}

