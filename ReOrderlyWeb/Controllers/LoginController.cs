using Microsoft.AspNetCore.Mvc;
using ReOrderlyWeb.Controllers;
using ReOrderlyWeb.SQL.Data;
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

        var token = TokenJWT.GenerateToken(user);
        return Ok(new { token });
    }
    
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return NoContent();
    }
    
}