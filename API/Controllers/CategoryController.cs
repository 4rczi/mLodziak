using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using SharedServices;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{userId}")]
        [Authorize(Policy = "AccessTokenPolicy")]
        public async Task<ActionResult<List<CategoryModel>>> GetCategories([FromRoute]string userId)
        {
            var categoryModels = await _categoryService.GetCategoryModelsAsync(userId);

            if (categoryModels == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(categoryModels);
        }
    }
}
