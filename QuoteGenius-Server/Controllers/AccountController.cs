using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using QuoteGenius_Server.DTOs;
using QuoteModel;
using System.IdentityModel.Tokens.Jwt;

namespace QuoteGenius_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<QuoteGeniusUser> _userManager;
        private readonly JwtHandler _jwtHandler;

        public AccountController(UserManager<QuoteGeniusUser> userManager, JwtHandler jwtHandler)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            QuoteGeniusUser? user = await _userManager.FindByNameAsync(loginRequest.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                return Unauthorized(new LoginResult
                {
                    Success = false,
                    Message = "Invalid Username or Password."
                });
            }

            JwtSecurityToken secToken = await _jwtHandler.GetTokenAsync(user);
            string? jwt = new JwtSecurityTokenHandler().WriteToken(secToken);
            return Ok(new LoginResult
            {
                Success = true,
                Message = "Login successful",
                Token = jwt
            });
        }

        [HttpGet("IsAdmin")]
        [Authorize]
        public Task<IActionResult> IsAdmin()
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var tokenData = _jwtHandler.GetPrincipalFromToken(token);
            var isAdmin = tokenData!.IsInRole("Administrator");

            return Task.FromResult<IActionResult>(Ok(new
            {
                IsAdmin = isAdmin
            }));
        }


    }
}
