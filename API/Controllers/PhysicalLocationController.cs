using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SharedModels;
using SharedServices;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhysicalLocationController : ControllerBase
    {
        private readonly IPhysicalLocationService _physicalLocationService;


        public PhysicalLocationController(IPhysicalLocationService physicalLocationService)
        {
            _physicalLocationService = physicalLocationService;
        }

        [HttpGet("{userId}/{categoryId}/{locationId}")]
        [Authorize(Policy = "AccessTokenPolicy")]
        public async Task<ActionResult<List<PhysicalLocationModel>>> GetPhysicalLocations(
            [FromRoute] string userId,
            [FromRoute] int categoryId,
            [FromRoute] int locationId)
        {
            var physicalLocationList = await _physicalLocationService.GetPhysicalLocationsAsync(userId, categoryId, locationId);

            if (physicalLocationList.IsNullOrEmpty())
            {
                return NotFound();
            }

            return Ok(physicalLocationList);
        }

        [HttpGet("{userId}/visitable")]
        [Authorize(Policy = "AccessTokenPolicy")]
        public async Task<ActionResult<List<PhysicalLocationModel>>> GetVisitablePhysicalLocations([FromRoute] string userId)
        {
            var physicalLocationList = await _physicalLocationService.GetVisitablePhysicalLocationsAsync(userId);

            if (physicalLocationList.IsNullOrEmpty())
            {
                return NotFound();
            }

            return Ok(physicalLocationList);
        }

        [HttpGet("{userId}/single/{physicalLocationId}")]
        [Authorize(Policy = "AccessTokenPolicy")]
        public async Task<ActionResult<PhysicalLocationModel>> GetSinglePhysicalLocation(
            [FromRoute] string userId,
            [FromRoute] int physicalLocationId)
        {
            var physicalLocation = await _physicalLocationService.GetPhysicalLocationAsync(physicalLocationId, userId);

            if (physicalLocation == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(physicalLocation);
        }
    }
}
