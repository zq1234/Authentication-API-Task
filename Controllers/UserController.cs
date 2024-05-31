using Authentication_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Authentication_API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
       
            [HttpGet("PlayerData")]
            [Authorize(Roles = "player,admin")]
            public  async Task<IActionResult> PlayerEndpoint()
            {
            var player = await Task.FromResult(new string[] { "Jack", "Joe", "Jill" });
            return Ok(player);
            }

            [HttpGet("AdminData")]
            [Authorize(Roles = "admin")]
            public IActionResult AdminEndpoint()
            {
                return Ok("This is an Admin endpoint");
            }
    }

}
