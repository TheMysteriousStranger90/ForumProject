using System;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Interfaces;
using ForumProjectWebAPI.Filters;
using ForumProjectWebAPI.Logs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ForumProjectWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ModelStateActionFilter]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;
        
        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        /// <summary>
        /// Authentication a user by provided credentials
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns>Session information, including access token</returns>
        /// <response code="200">Successfully authenticated</response>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                var authResponse = await _authenticationService.LoginAsync(loginDTO);
                _logger.LogInformation("User with email {Email} signed in successfully", loginDTO.Email);
                return Ok(authResponse.Token);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="signUpDTO"></param>
        /// <returns>Return nothing, your registration is successful</returns>
        /// <response code="204">Return nothing</response>
        [HttpPost("Registration")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Register(SignUpDTO signUpDTO)
        {
            try
            {
                await _authenticationService.SignUpAsync(signUpDTO);
                _logger.LogInformation("User with email {Email} signed up successfully", signUpDTO.Email);
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