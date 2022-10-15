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
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;
        private readonly ILogger<TopicController> _logger;

        public TopicController(ITopicService topicService, ILogger<TopicController> logger)
        {
            _topicService = topicService;
            _logger = logger;
        }
        
        [HttpGet("GetTopicByCategoryId/{id}")]
        [Authorize]
        public IActionResult GetTopicByCategoryId(int categoryId)
        {
            try
            {
                var topic = _topicService.GetByCategoryId(categoryId);
                return Ok(topic);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("GetTopicByUserId/{id}")]
        [Authorize]
        public IActionResult GetTopicByUserId(int userId)
        {
            try
            {
                var topic = _topicService.GetByUserId(userId);
                return Ok(topic);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("GetAllTopic")]
        [Authorize]
        public async Task<IActionResult> GetAllTopic()
        {
            try
            {
                var topics = await _topicService.GetAllTopics();
                return Ok(topics);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("GetAllWithDetails")]
        [Authorize]
        public async Task<IActionResult> GetAllWithDetails()
        {
            try
            {
                var topics = await _topicService.GetAllWithDetailsAsync();
                return Ok(topics);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("GetTopicById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetTopicById(int id)
        {
            try
            {
                var user = await _topicService.GetByIdAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("GetByIdWithDetails/{id}")]
        [Authorize]
        public async Task<IActionResult> GetByIdWithDetails(int id)
        {
            try
            {
                var user = await _topicService.GetByIdWithDetailsAsync(id);
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
        public async Task<IActionResult> CreateTopic(TopicCreateDTO model)
        {
            try
            {
                var createdTopic = await _topicService.CreateAsync(model);
                _logger.LogInformation("Created topic with id {TopicId}",
                    createdTopic.Id);

                return CreatedAtAction(nameof(GetTopicById), new { id = createdTopic.Id }, createdTopic);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut("UpdateTopic")]
        [Authorize(Roles = RoleTypes.Admin + "," + RoleTypes.Moderator)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateTopic(TopicDTO model)
        {
            try
            {
                var userId = Convert.ToInt32(User.FindFirstValue("UserId"));
                await _topicService.UpdateAsync(model, userId);
                _logger.LogInformation("User with Id {UserId} changed topic with id {TopicId} successfully",
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
        public async Task<IActionResult> RemoveTopic(int id)
        {
            try
            {
                await _topicService.DeleteAsync(id);
                _logger.LogInformation("Removed topic with id {Id} successfully", id);
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