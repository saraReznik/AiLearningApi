using BL.Api;
using BL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IBLUser _userService;

    public AuthController(IConfiguration configuration, IBL bl)
    {
        _configuration = configuration;
        _userService = bl.User;
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Phone { get; set; } 
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _userService.ReadByEmail(request.Email);

        if (user != null && user.Phone == request.Phone)
        {
            var token = GenerateJwtToken(user);
            return Ok(new { token, user });
        }

        return Unauthorized("Email or Phone are incorrect.");
    }

    private string GenerateJwtToken(BLUser user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");

        var claims = new[]
        {
            new Claim("id", user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role ?? "User")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(3),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}