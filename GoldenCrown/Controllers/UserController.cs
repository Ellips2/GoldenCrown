using GoldenCrown.DTOs;
using GoldenCrown.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoldenCrown.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]  // POST api/user/register
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _userService.RegisterAsync(request.Login, request.Name, request.Password);
            if (result)
            {
                return Ok();
            }
            return BadRequest(new { Message = "User registration failed" });
        }

        //[HttpGet("{userId}")]   //GET api/user/{userId}=123
        //public Task<IActionResult> GetUserDetails([FromQuery] int userId, )
        //{
        //    var userDetails = new
        //    {
        //        UserId = userId,
        //        Username = "SampleUser",
        //        Email = ""
        //    };
        //    return Task.FromResult<IActionResult>(Ok(userDetails));
        //}
    }
}
