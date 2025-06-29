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

        // POST: api/Prompt
        // [הערה]: פונקציה זו נשארת ASYNC כי היא קוראת ל-ProcessPromptAsync,
        // שבעצמו קורא ל-OpenAI דרך הרשת (פעולה אסינכרונית).
        [HttpPost]
        public async Task<IActionResult> CreatePrompt([FromBody] BLPrompt prompt)
        {
            // שגיאות עלולות להיזרק מכאן, והן יטופלו על ידי ה-ErrorHandlingMiddleware
            var result = await _promptService.ProcessPromptAsync(prompt);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/all")]
        public IActionResult GetAllPromptsForAdmin() // [תיקון]: הוסר async Task
        {
            // [תיקון]: קריאה לפונקציה הסינכרונית
            var prompts = _promptService.GetAll();
            return Ok(prompts);
        }

        // GET: api/Prompt/user/5
        [HttpGet("user/{userId}")]
        public IActionResult GetPromptsByUser(int userId) // [תיקון]: הוסר async Task
        {
            // [תיקון]: קריאה לפונקציה הסינכרונית
            var prompts = _promptService.GetPromptsByUserId(userId);
            return Ok(prompts);
        }

        // GET: api/Prompt/user/5/category/2
        [HttpGet("user/{userId}/category/{categoryId}")]
        public IActionResult GetPromptsByUserAndCategory(int userId, int categoryId) // [תיקון]: הוסר async Task
        {
            // [תיקון]: קריאה לפונקציה הסינכרונית
            var prompts = _promptService.GetPromptsByUserIdAndCategory(userId, categoryId);
            return Ok(prompts);
        }

        // GET: api/Prompt/5
        [HttpGet("{id}")]
        public IActionResult GetPromptById(int id) // [תיקון]: הוסר async Task
        {
            // [תיקון]: קריאה לפונקציה הסינכרונית
            var prompt = _promptService.Read(id);
            if (prompt == null)
            {
                return NotFound();
            }
            return Ok(prompt);
        }

        // PUT: api/Prompt/5
        [HttpPut("{id}")]
        public IActionResult UpdatePrompt(int id, [FromBody] BLPrompt prompt) // [תיקון]: הוסר async Task
        {
            if (id != prompt.Id)
                return BadRequest("ID mismatch");

            // [תיקון]: קריאה לפונקציה הסינכרונית
            _promptService.Update(prompt);
            return NoContent();
        }

        // DELETE: api/Prompt/5
        [HttpDelete("{id}")]
        public IActionResult DeletePrompt(int id) // [תיקון]: הוסר async Task
        {
            // [תיקון]: קריאה לפונקציה הסינכרונית
            _promptService.Delete(id);
            return NoContent();
        }
    }
}