using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReOrderlyWeb.SQL.Data;
using ReOrderlyWeb.SQL.Data.DAO;
using ReOrderlyWeb.ViewModels;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ReOrderlyWeb.Controllers;

public class LoginController : ControllerBase
{
    private readonly ReOrderlyWebDbContext _context;
    
    public LoginController(ReOrderlyWebDbContext context)
    {
        _context = context;
    }   
    
    
    //XXXXXXXXXXDDDDDDDDDDDDDDDDDDDDDD TODO: ZMIENIC TAK ZEBY WSZYSTKO BYLO NA JWT, CALKOWICIE USUNAC COOKIES.
    //logowanie do konta.
    [HttpPost("login")]
    public async Task<IActionResult> Post([FromBody] LoginViewModel login)
    {
        var user = _context.User.SingleOrDefault(c => c.emailAddress == login.emailAddress);

        var md5pass = md5Convert.md5gen(login.password);
        Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" + login.emailAddress + login.password, md5pass);
        if (user == null || user.password != md5pass)
        {
                
            return Unauthorized();
                
        }
        var claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim("UserId", user.userId.ToString()),
            new Claim(ClaimTypes.Name, user.name), 
            new Claim(ClaimTypes.Email, user.emailAddress),  
            
        }, "Cookies");

        
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        
        await Request.HttpContext.SignInAsync("Cookies", claimsPrincipal);

        
        var token = GenerateToken(user); 
        return Ok(new { token });
        
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return NoContent();
    }
    
    private string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.emailAddress),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("UserId", user.userId.ToString()),
            new Claim(ClaimTypes.Name, user.name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KLUCZDOTESTOWTESTTESTTESTTESTTESTTESTTESTTESTTESTTEST")); 
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