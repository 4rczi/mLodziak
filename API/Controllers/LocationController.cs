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

        [HttpGet("locations")]
        [Authorize(Policy = "AccessTokenPolicy")]
        public async Task<ActionResult<List<LocationModel>>> GetLocations([FromQuery] int categoryId, [FromQuery] string userId)
        {
            var locationModels = await _locationService.GetLocationModelsAsync(userId, categoryId);

            if (locationModels.IsNullOrEmpty())
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return Ok(locationModels);
        }

        [HttpGet("all")]
        [Authorize(Policy = "AccessTokenPolicy")]
        public async Task<ActionResult<Dictionary<int, List<LocationModel>>>> GetAllLocations([FromQuery] string userId)
        {
            var locationModels = await _locationService.GetAllLocationModelsAsync(userId);

            if (locationModels.IsNullOrEmpty())
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            return Ok(locationModels);
        }
    }
}

