using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Repositories;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationLoggingController : ControllerBase
    {
        private readonly IApplicationLoggingRepository _applicationLoggingRepository;


        public ApplicationLoggingController(IApplicationLoggingRepository applicationLoggingRepository)
        {
            _applicationLoggingRepository = applicationLoggingRepository;
        }

        [HttpPost()]
        public async Task<ActionResult<User>> CreateLog([FromBody] ApplicationLogging logEntity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _applicationLoggingRepository.CreateApplicationLoggingAsync(logEntity);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
