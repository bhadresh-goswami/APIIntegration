using API.Core;
using API.Infrastructure.Interface;
using LoginDemoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoginDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthController(UserManager<AppUser> userManager, IConfiguration configuration, IUserRepository userRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            ApiResponse<AppUser> response;
            try
            {
                var user = new AppUser { UserName = model.Username, Email = model.Email };
                var returnUser = await _userRepository.CreateUser(user, model.Password);

                response = new ApiResponse<AppUser>(true, "", returnUser);

            }
            catch (Exception ex)
            {
                response = new ApiResponse<AppUser>(false, ex.Message, null);

            }


            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Validate user credentials
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                // Generate JWT token
                var token = GenerateJwtToken(user);
                return Ok(new { token });
            }
            else
            {
                return Unauthorized();
            }
        }

        private string GenerateJwtToken(AppUser user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:TokenExpirationDays"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
