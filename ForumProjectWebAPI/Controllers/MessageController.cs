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