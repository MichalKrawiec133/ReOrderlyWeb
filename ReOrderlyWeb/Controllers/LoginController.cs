using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ReOrderlyWeb.SQL.Data;
using ReOrderlyWeb.ViewModels;

namespace ReOrderlyWeb.Controllers;

public class LoginController : ControllerBase
{
    private readonly ReOrderlyWebDbContext _context;
    
    public LoginController(ReOrderlyWebDbContext context)
    {
        _context = context;
    }   
    
    [HttpPost("login")]
    public async Task<IActionResult> Post([FromBody] LoginViewModel login)
    {
        var user = _context.User.SingleOrDefault(c => c.emailAddress == login.emailAddress);

        var md5pass = md5Convert.md5gen(login.password);
        
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

        
        return NoContent();
        
    }
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return NoContent();
    }
    
    
}