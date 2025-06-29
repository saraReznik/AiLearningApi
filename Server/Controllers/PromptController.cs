using BL.Api;
using BL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromptController : ControllerBase
    {
        private readonly IBLPrompt _promptService;

        public PromptController(IBLPrompt promptService)
        {
            _promptService = promptService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrompt([FromBody] BLPrompt prompt)
        {
            var result = await _promptService.ProcessPromptAsync(prompt);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/all")]
        public IActionResult GetAllPromptsForAdmin() 
        {
            var prompts = _promptService.GetAll();
            return Ok(prompts);
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetPromptsByUser(int userId) 
        {
            var prompts = _promptService.GetPromptsByUserId(userId);
            return Ok(prompts);
        }

        [HttpGet("user/{userId}/category/{categoryId}")]
        public IActionResult GetPromptsByUserAndCategory(int userId, int categoryId) 
        {
            var prompts = _promptService.GetPromptsByUserIdAndCategory(userId, categoryId);
            return Ok(prompts);
        }

        [HttpGet("{id}")]
        public IActionResult GetPromptById(int id) 
        {
            var prompt = _promptService.Read(id);
            if (prompt == null)
            {
                return NotFound();
            }
            return Ok(prompt);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePrompt(int id, [FromBody] BLPrompt prompt) 
        {
            if (id != prompt.Id)
                return BadRequest("ID mismatch");

            _promptService.Update(prompt);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePrompt(int id) 

        {
            _promptService.Delete(id);
            return NoContent();
        }
    }
}