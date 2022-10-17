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
    /// Message controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ModelStateActionFilter]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly ILogger<MessageController> _logger;

        public MessageController(IMessageService messageService, ILogger<MessageController> logger)
        {
            _messageService = messageService;
            _logger = logger;
        }

        /// <summary>
        /// Get all messages by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Return all messages by user id</returns>
        [HttpGet("FindAllByUserId/{id}")]
        [Authorize]
        public IActionResult FindAllByUserId(int userId)
        {
            try
            {
                var messages = _messageService.FindAllByUserId(userId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get messages by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Again! Return all messages by user id</returns>
        [HttpGet("FindByUserId/{id}")]
        [Authorize]
        public IActionResult FindByUserId(int userId)
        {
            try
            {
                var messages = _messageService.FindByUserId(userId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get all messages
        /// </summary>
        /// <returns>Return all messages</returns>
        [HttpGet("GetAllMessage")]
        [Authorize]
        public async Task<IActionResult> GetAllMessage()
        {
            try
            {
                var messages = await _messageService.GetAllMessages();
                return Ok(messages);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a messages with details by topic id
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns>The messages with details specified by topic<paramref name="topicId"/></returns>
        [HttpGet("GetByTopicIdWithDetails")]
        [Authorize]
        public async Task<IActionResult> GetByTopicIdWithDetails(int topicId)
        {
            try
            {
                var messages = await _messageService.GetByTopicIdWithDetailsAsync(topicId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a message by id
        /// </summary>
        /// <param name="id">Id of the message to be retrieved</param>
        /// <returns>The message specified by <paramref name="id"/></returns>
        [HttpGet("GetMessageById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetMessageById(int id)
        {
            try
            {
                var message = await _messageService.GetByIdAsync(id);
                return Ok(message);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create new message
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Newly created message</returns>
        /// <response code="201">Returns the newly created message</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateMessage(MessageCreateDTO model)
        {
            try
            {
                var createdMessage = await _messageService.CreateAsync(model);
                _logger.LogInformation("Created message with id {MessageId}",
                    createdMessage.Id);

                return CreatedAtAction(nameof(GetMessageById), new { id = createdMessage.Id }, createdMessage);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// This method updates information by message
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Update</returns>
        /// <response code="204">Return nothing, message has been updated</response>
        [HttpPut("UpdateMessage")]
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.Moderator)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateMessage(MessageDTO model)
        {
            try
            {
                var userId = Convert.ToInt32(User.FindFirstValue("UserId"));
                await _messageService.UpdateAsync(model, userId);
                _logger.LogInformation("User with Id {UserId} changed message with id {MessageId} successfully",
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
        /// Delete the message
        /// </summary>
        /// <param name="id">ID of the message to be deleted</param>
        /// <returns>Delete</returns>
        /// <response code="204">Message has been deleted</response>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.Moderator)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveMessage(int id)
        {
            try
            {
                await _messageService.DeleteAsync(id);
                _logger.LogInformation("Removed message with id {Id} successfully", id);
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