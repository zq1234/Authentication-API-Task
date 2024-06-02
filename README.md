# Authentication API

# OAuth Login API

This project is an example of an OAuth login API built with ASP.Net Core MVC. The API authenticates users and issues JWT tokens containing user roles and accessible system regions. 

## Features

- Username and password authentication
- JWT token generation with roles and system regions in claims
- Static data for users, roles, and regions (no database)

## System Roles and Regions

### Regions
1. Board game `b_game` (default for all logged in users)
2. VIP Character modification `vip_chararacter_personalize` (For VIP users)

### Roles
1. Player `player`
2. Administrator `admin`

## Prerequisites

- .NET 6.0 SDK or later

## Getting Started

### Clone the Repository

```sh
git clone https://github.com/yourusername/oauth-login-api.git
cd oauth-login-api


POST /api/auth/login
Request
{
  "username": "exampleUser",
  "password": "examplePassword"
}
Response
json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}

  
//////////////////////////////////////////////////////////////////////////////////////////////

Example Users
Player

Username: player
Password: player123
Role: player
Regions: b_game


VIP Player
Username: admin
Password: admin123
Role: admin
Regions: b_game, vip_chararacter_personalize





Implementation Details

AuthController

using Authentication_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
namespace Authentication_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginViewModel model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtToken:Secret"]));

            var (email, userName, roles, scopes) = StaticUserStore.ValidateUser(model.Email, model.Password);

            if (email != null && userName != null)
            {
                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, email)
            };

                foreach (var role in roles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                foreach (var scope in scopes)
                {
                    authClaims.Add(new Claim("scope", scope));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(authClaims),
                    Expires = DateTime.Now.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new
                {
                    Email = email,
                    Roles = roles,
                    Scopes = scopes,
                    Token = tokenString,
                    Message = "Success"
                });
            }
            else
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }
        }


    }

}


UserController

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


Models


namespace Authentication_API.Models
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}


public static class StaticUserStore
{
    private static List<User> users = new List<User>
    {
        new User { Email = "player@gmail.com", UserName = "player", Password = "player123", Roles = new[] { "player" }, Scopes = new[] { "b_game" } },
        new User { Email = "admin@gmail.com", UserName = "admin", Password = "admin123", Roles = new[] { "admin" }, Scopes = new[] { "b_game", "vip_chararacter_personalize" } }
    };

    public static (string Email, string UserName, IEnumerable<string> Roles, IEnumerable<string> Scopes) ValidateUser(string emailOrUserName, string password)
    {
        var user = users.FirstOrDefault(u => (u.Email == emailOrUserName || u.UserName == emailOrUserName) && u.Password == password);

        if (user != null)
        {
            return (user.Email, user.UserName, user.Roles, user.Scopes);
        }

        return (null, null, null, null);
    }

    private class User
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<string> Scopes { get; set; }
    }
}




  

