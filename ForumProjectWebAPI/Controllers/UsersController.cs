using System;
using System.Security.Claims;
using System.Threading.Tasks;
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
    [Route("api/[controller]")]
    [ApiController]
    [ModelStateActionFilter]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly JwtConfig _jwtConfig;

        public UsersController(ILogger<UsersController> logger, IUserService userService, IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userService = userService;
            _logger = logger;
            _jwtConfig = optionsMonitor.CurrentValue;
        }
        
        [HttpGet("{id:int}")]
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