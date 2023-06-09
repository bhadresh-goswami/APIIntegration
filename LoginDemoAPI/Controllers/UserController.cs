using API.Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoginDemoAPI.Controllers
{
    [Authorize]

    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            // Retrieve user information from the repository
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            // Return user information
            return Ok(user);
        }
    }

}
