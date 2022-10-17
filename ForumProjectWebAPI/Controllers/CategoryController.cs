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
    /// <summary>
    /// Category controller
    /// </summary>
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

        /// <summary>
        /// Get all category
        /// </summary>
        /// <returns>Return all categories</returns>
        [HttpGet("GetAllCategory")]
        [Authorize]
        public async Task<IActionResult> GetAllCategory()
        {
            try
            {
                var categories = await _categoryService.GetAllCategory();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a category by id
        /// </summary>
        /// <param name="id">Id of the category to be retrieved</param>
        /// <returns>The category specified by <paramref name="id"/></returns>
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

        /// <summary>
        /// Create new category
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Newly created category</returns>
        /// <response code="201">Returns the newly created category</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateCategory(CategoryCreateDTO model)
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

        /// <summary>
        /// This method updates information by category
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Update</returns>
        /// <response code="204">Return nothing, category has been updated</response>
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

        /// <summary>
        /// Delete the category
        /// </summary>
        /// <param name="id">ID of the category to be deleted</param>
        /// <returns>Delete</returns>
        /// <response code="204">Category has been deleted</response>
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