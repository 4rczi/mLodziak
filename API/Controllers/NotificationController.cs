using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {

        private readonly FCMService _FCMService;
        public NotificationController(FCMService FCMService)
        {
            _FCMService = FCMService;
        }

        [HttpPost()]
        [Authorize(Policy = "AccessTokenPolicy")]
        public async Task<ActionResult<string>> CreateNotification([FromBody] NotificationRequestModel notificationRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _FCMService.SendNotificationAsync(notificationRequest);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
