using System;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Injections;
using BLL.Interfaces;
using DAL.Entities.Roles;
using ForumProjectWebAPI.Filters;
using ForumProjectWebAPI.Logs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ForumProjectWebAPI.Controllers
{
    /// <summary>
    /// Users controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ModelStateActionFilter]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>Users</returns>
        /// <response code="200">Return all users</response>
        [HttpGet("GetAllUsers")]
        [Authorize(Roles = RoleTypes.Admin)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">Id of the user to be retrieved</param>
        /// <returns>User</returns>
        /// <response code="200">Return user by id</response>
        [HttpGet("GetUserById/{id}")]
        [Authorize(Roles = RoleTypes.Admin)]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email">Email of the user to be retrieved</param>
        /// <returns>User</returns>
        /// <response code="200">Return user by email</response>
        [HttpGet("GetUserByEmail")]
        [Authorize(Roles = RoleTypes.Admin)]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                return Ok(user);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get user roles by email
        /// </summary>
        /// <param name="email">Username of the user's roles to be retrieved</param>
        /// <returns>User roles</returns>
        /// <response code="200">Return user roles by email</response>
        [HttpGet("GetUserRoles")]
        [Authorize(Roles = RoleTypes.Admin)]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            try
            {
                var result = await _userService.GetUserRoles(email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// This method is for applying role to users
        /// </summary>
        /// <param name="userRoleDTO"></param>
        /// <returns>Set user role</returns>
        /// <response code="204">Return nothing</response>
        [HttpPost("SetUserRole")]
        [Authorize(Roles = RoleTypes.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SetUserRole(UserRoleDTO userRoleDTO)
        {
            try
            {
                var userId = Convert.ToInt32(User.FindFirstValue("UserId"));
                await _userService.SetUserRoleAsync(userRoleDTO);

                _logger.LogInformation("Admin with Id {UserId} changed role of user with id {Id} to {Role}",
                    userId, userRoleDTO.Id, userRoleDTO.Role);
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// This method updates information by user
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns>Update</returns>
        /// <response code="204">Return nothing, user has been updated</response>
        [HttpPut("UpdateUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateUser(UserDTO userDTO)
        {
            try
            {
                var userId = Convert.ToInt32(User.FindFirstValue("UserId"));
                await _userService.UpdateAsync(userDTO, userId);

                _logger.LogInformation("User with Id {UserId} changed its profile successfully", userId);
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete the user
        /// </summary>
        /// <param name="id">ID of the user to be deleted</param>
        /// <returns>Delete</returns>
        /// <response code="204">User has been deleted</response>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = RoleTypes.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveUser(int id)
        {
            try
            {
                await _userService.DeleteByIdAsync(id);
                _logger.LogInformation("Admin removed user with id {Id} successfully", id);
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