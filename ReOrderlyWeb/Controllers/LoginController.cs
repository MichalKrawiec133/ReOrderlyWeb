using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ReOrderlyWeb.Controllers;
using ReOrderlyWeb.SQL.Data;
using ReOrderlyWeb.SQL.Data.DAO;
using ReOrderlyWeb.ViewModels;

public class LoginController : ControllerBase
{
    private readonly ReOrderlyWebDbContext _context;

    public LoginController(ReOrderlyWebDbContext context)
    {
        _context = context;
    }   

    // logowanie do konta z uzyciem jwt
    [HttpPost("login")]
    public IActionResult Post([FromBody] LoginViewModel login)
    {
        var user = _context.User.SingleOrDefault(c => c.emailAddress == login.emailAddress);

        var md5pass = md5Convert.md5gen(login.password);
        if (user == null || user.password != md5pass)
        {
            return Unauthorized();
        }
        
        var token = GenerateToken(user);
        return Ok(new { token });
    }
    
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return NoContent();
    }

    //generacja tokenu
    private string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.emailAddress),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("UserId", user.userId.ToString()),
            new Claim(ClaimTypes.Name, user.name),
            new Claim(ClaimTypes.Email, user.emailAddress)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KLUCZDOTESTOWTESTTESTTESTTESTTESTTEST")); 
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "API",
            audience: "USER", 
            claims: claims,
            expires: DateTime.Now.AddMinutes(30), 
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}