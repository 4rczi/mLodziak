using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using SharedServices;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserHistoryController : ControllerBase
    {
        private readonly IUserHistoryService _userHistoryService;


        public UserHistoryController(IUserHistoryService userHistoryService)
        {
            _userHistoryService = userHistoryService;
        }

        [HttpPost()]
        [Authorize(Policy = "AccessTokenPolicy")]
        public async Task<ActionResult<bool>> CreateUserHistoryAsync([FromBody] UserHistory userHistoryEntity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _userHistoryService.CreateUserHistoryAsync(userHistoryEntity);

                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
