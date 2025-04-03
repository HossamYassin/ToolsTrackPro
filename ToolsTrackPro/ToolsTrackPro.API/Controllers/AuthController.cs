using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToolsTrackPro.API.Models;
using ToolsTrackPro.Application.DTOs;
using ToolsTrackPro.Application.Features.Users.Commands;

namespace ToolsTrackPro.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        public AuthController(IMediator mediator, IConfiguration configuration)
        {
            _configuration = configuration;
            _mediator = mediator;
        }

        /// <summary>
        /// user login 
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _mediator.Send(new UserLoginCommand() { Email = request.Email, Password = request.Password});
            
            if (user is null)
                return Unauthorized((new ApiResponse<object>("fail", new List<string> { "Invalid credentials" })));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);

            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { Token = tokenHandler.WriteToken(token) });
        }

        /// <summary>
        /// Add a new user
        /// </summary>         
        [Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> Add([FromBody] UserDto user)
        {
            var added = await _mediator.Send(new AddUserCommand(user));
            return Ok(new ApiResponse<UserDto>(added ? "success" : "fail"));
        }
    }
}
