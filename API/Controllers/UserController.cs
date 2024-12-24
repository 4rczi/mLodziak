using SharedServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using DataAccess.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}")]
        [Authorize(Policy = "AccessTokenPolicy")]
        public async Task<ActionResult<User>> GetUser([FromRoute]string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost()]
        [Authorize(Policy = "AccessTokenPolicy")]
        public async Task<ActionResult<User>> CreateUser([FromBody] User userEntity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _userService.CreateUserAsync(userEntity);

                var newUser = await _userService.GetUserByIdAsync(userEntity.Id);
                if (newUser == null)
                {
                    return BadRequest("User creation failed");
                }

                return CreatedAtAction(nameof(GetUser), new { userId = newUser.Id }, newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
