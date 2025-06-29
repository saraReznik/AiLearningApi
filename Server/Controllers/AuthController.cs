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
    private readonly IBLUser _userService; // הוספנו את שירות המשתמשים

    // הזרקנו את שירות המשתמשים דרך ה-Constructor
    public AuthController(IConfiguration configuration, IBL bl)
    {
        _configuration = configuration;
        _userService = bl.User;
    }

    // המודל שמגיע מהפרונטאנד
    public class LoginRequest
    {
        public string Email { get; set; } // שינינו חזרה ל-Email כדי שיתאים לטופס
        public string Phone { get; set; }
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // שלב 1: מנסים למצוא משתמש לפי האימייל שנשלח
        // הערה: ודאי שהפונקציה ReadByEmail קיימת ופועלת בשירות המשתמשים שלך
        var user = _userService.ReadByEmail(request.Email);

        // שלב 2: בודקים אם המשתמש קיים והאם הטלפון תואם
        // (אפשר להחליף את הטלפון בבדיקת סיסמה מוצפנת בעתיד)
        if (user != null && user.Phone == request.Phone)
        {
            // אם כן, המשתמש אומת בהצלחה!
            // עכשיו נייצר לו טוקן אישי
            var token = GenerateJwtToken(user);

            // מחזירים גם את הטוקן וגם את פרטי המשתמש לפרונטאנד
            return Ok(new { token, user });
        }

        // אם המשתמש לא נמצא או שהטלפון שגוי
        return Unauthorized("Email or Phone are incorrect.");
    }

    // הפונקציה הזו יוצרת טוקן עם פרטי המשתמש הספציפי
    private string GenerateJwtToken(BLUser user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");

        var claims = new[]
        {
            new Claim("id", user.UserId.ToString()), // ה-ID של המשתמש
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            // אפשר להוסיף גם תפקיד אם יש לך
            // new Claim(ClaimTypes.Role, user.Role) 
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