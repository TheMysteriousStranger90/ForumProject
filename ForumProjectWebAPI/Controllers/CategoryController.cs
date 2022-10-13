using System;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities.Roles;
using ForumProjectWebAPI.Filters;
using ForumProjectWebAPI.Logs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ForumProjectWebAPI.Controllers
{
    //need to fix...
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ModelStateActionFilter]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet("GetAllCategory")]
        [Authorize]
        public async Task<IActionResult> GetAllCategory()
        {
            try
            {
                var categories = _categoryService.GetAllCategory();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCategoryById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var user = await _categoryService.GetByIdAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCategory(CategoryDTO model)
        {
            try
            {
                var createdCategory = await _categoryService.CreateAsync(model);
                _logger.LogInformation("Created category with id {CategoryId}",
                    createdCategory.Id);

                return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateCategory")]
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.Moderator)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateCategory(CategoryDTO model)
        {
            try
            {
                var userId = Convert.ToInt32(User.FindFirstValue("UserId"));
                await _categoryService.UpdateAsync(model, userId);
                _logger.LogInformation("User with Id {UserId} changed category with id {CategoryId} successfully",
                    userId, model.Id);

                return NoContent();
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.Moderator)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveCategory(int id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);
                _logger.LogInformation("Removed category with id {Id} successfully", id);
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }
    }
}