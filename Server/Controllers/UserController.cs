using BL.Api;
using BL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IBLUser _userService;

        public UserController(IBL BL)
        {
            _userService = BL.User;
        }

        // POST /api/User
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<BLUser> Create([FromBody] BLUser user)
        {
            if (user == null)
            {
                return BadRequest("User object is null");
            }

            _userService.Create(user); 
            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin/all")]
        public IActionResult GetAll()
        {
            return Ok(_userService.GetAll());
        }

        [Authorize]
        [HttpPut]
        public ActionResult<BLUser> Update([FromBody] BLUser user)
        {
            if (user == null)
            {
                return BadRequest("User cannot be null.");
            }

            _userService.Update(user); 

            return Ok(user);
        }
    }
}