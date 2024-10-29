using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ReOrderlyWeb.SQL.Data;
using ReOrderlyWeb.SQL.Data.DAO;
using ReOrderlyWeb.ViewModels;

namespace ReOrderlyWeb.Controllers;


[ApiController]
public class RegisterController: ControllerBase
{
    
    
    private readonly ReOrderlyWebDbContext _context;

    public RegisterController(ReOrderlyWebDbContext context)
    {
        _context = context;
    }   
    
    [HttpPost("account/register")]
    public async Task<IActionResult> Register([FromBody] UserViewModel model)
    {
        if (string.IsNullOrEmpty(model.emailAddress) || string.IsNullOrEmpty(model.password))
        {
            return BadRequest("Email and password are required.");
        }

       
        var existingUser = _context.User.SingleOrDefault(u => u.emailAddress == model.emailAddress);
        if (existingUser != null)
        {
            return Conflict("A user with this email already exists.");
        }

        
        var hashedPassword = md5Convert.md5gen(model.password);

        
        var newUser = new User
        {
            name = model.name,
            lastName = model.lastName,
            streetName = model.streetName,
            houseNumber = model.houseNumber,
            voivodeship = model.voivodeship,
            country = model.country,
            zipcode = model.zipcode,
            emailAddress = model.emailAddress,
            password = hashedPassword,
            phoneNumber = model.phoneNumber
        };

        _context.User.Add(newUser);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Registration successful." });
    }

}