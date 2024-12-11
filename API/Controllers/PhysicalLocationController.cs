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

        [HttpGet()]
        [Authorize(Policy = "AccessTokenPolicy")]
        public async Task<ActionResult<List<PhysicalLocationModel>>> GetPhysicalLocations([FromQuery]string userId, [FromQuery]int categoryId, [FromQuery]int locationId)
        {
            var physicalLocationList = await _physicalLocationService.GetPhysicalLocationsAsync(userId, categoryId, locationId);

            if (physicalLocationList.IsNullOrEmpty())
            {
                return NotFound();
            }

            return Ok(physicalLocationList);
        }

        [HttpGet("visitable")]
        [Authorize(Policy = "AccessTokenPolicy")]
        public async Task<ActionResult<List<PhysicalLocationModel>>> GetVisitablePhysicalLocations([FromQuery]string userId)
        {
            var physicalLocationList = await _physicalLocationService.GetVisitablePhysicalLocationsAsync(userId);
        
            if (physicalLocationList.IsNullOrEmpty())
            {
                return NotFound();
            }
        
            return Ok(physicalLocationList);
        }
    }
}
