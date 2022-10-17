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
    /// Topic controller
    /// </summary>
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

        /// <summary>
        /// Get topics by category id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>Return all topics by category id</returns>
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

        /// <summary>
        /// Get topics by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Return all topics by user id</returns>
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

        /// <summary>
        /// Get all topics
        /// </summary>
        /// <returns>Return all topics</returns>
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

        /// <summary>
        /// Get all topics with details
        /// </summary>
        /// <returns>Return all topics with details</returns>
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

        /// <summary>
        /// Get a topic by id
        /// </summary>
        /// <param name="id">Id of the topic to be retrieved</param>
        /// <returns>The topic specified by <paramref name="id"/></returns>
        [HttpGet("GetTopicById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetTopicById(int id)
        {
            try
            {
                var topic = await _topicService.GetByIdAsync(id);
                return Ok(topic);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a topic with details by id
        /// </summary>
        /// <param name="id">Id of the topic to be retrieved</param>
        /// <returns>The topic with details specified by <paramref name="id"/></returns>
        [HttpGet("GetByIdWithDetails/{id}")]
        [Authorize]
        public async Task<IActionResult> GetByIdWithDetails(int id)
        {
            try
            {
                var topic = await _topicService.GetByIdWithDetailsAsync(id);
                return Ok(topic);
            }
            catch (Exception ex)
            {
                LogInfo.LogInfoMethod(ex, _logger);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create new topic
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Newly created topic</returns>
        /// <response code="201">Returns the newly created topic</response>
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

        /// <summary>
        /// This method updates information by topic
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Update</returns>
        /// <response code="204">Return nothing, topic has been updated</response>
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

        /// <summary>
        /// Delete the topic
        /// </summary>
        /// <param name="id">ID of the topic to be deleted</param>
        /// <returns>Delete</returns>
        /// <response code="204">Topic has been deleted</response>
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