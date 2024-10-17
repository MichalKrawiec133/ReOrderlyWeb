using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReOrderlyWeb.SQL.Data;
using ReOrderlyWeb.SQL.Data.DAO;
using ReOrderlyWeb.ViewModels;

namespace ReOrderlyWeb.Controllers;

public class UserAccountController : ControllerBase
{
    private readonly ReOrderlyWebDbContext _context;

    public UserAccountController(ReOrderlyWebDbContext context)
    {
        _context = context;
    }
    
    //XXXXXXXXXXDDDDDDDDDDDDDDDDDDDDDD TODO: ZMIENIC TAK ZEBY WSZYSTKO BYLO NA JWT, CALKOWICIE USUNAC COOKIES.  
    //tworzenie konta.

    [HttpPost("account")]
    public async Task<IActionResult> CreateUser([FromBody] UserViewModel userCreate)
    {
        
        var user = _context.User.SingleOrDefault(c => c.emailAddress == userCreate.emailAddress);
        //bool added = true;
        if (user != null)
        {
            return Conflict("User with this email already exists.");
        }

        var userAdd = new User();
        if (!string.IsNullOrEmpty(userCreate.name))
        {
            userAdd.name = userCreate.name;
        }

        if (!string.IsNullOrEmpty(userCreate.lastName))
        {
            userAdd.lastName = userCreate.lastName;
        }

        if (!string.IsNullOrEmpty(userCreate.streetName))
        {
            userAdd.streetName = userCreate.streetName;
        }

        if (userCreate.houseNumber.HasValue)
        {
            userAdd.houseNumber = userCreate.houseNumber.Value;
        }

        if (!string.IsNullOrEmpty(userCreate.voivodeship))
        {
            userAdd.voivodeship = userCreate.voivodeship;
        }

        if (!string.IsNullOrEmpty(userCreate.country))
        {
            userAdd.country = userCreate.country;
        }

        if (userCreate.zipcode.HasValue)
        {
            userAdd.zipcode = userCreate.zipcode.Value;
        }
        if (!string.IsNullOrEmpty(userCreate.emailAddress))
        {
            userAdd.emailAddress = userCreate.emailAddress;
        }

        if (!string.IsNullOrEmpty(userCreate.password))
        {
            var md5pass = md5Convert.md5gen(userCreate.password);
            if (userAdd.password != md5pass)
            {
                userAdd.password = md5pass;
            }
        }

        if (userCreate.phoneNumber != 0) 
        {
            userAdd.phoneNumber = userCreate.phoneNumber;
        }

       
        _context.User.Update(userAdd);
        await _context.SaveChangesAsync();


        return Ok();
        
        
    }
    
    
    //edycja danych usera.
    [HttpPatch("account")]
    [Authorize]
    public async Task<IActionResult> EditUserData([FromBody] UserViewModel userUpdated)
    {
       
        var user = _context.User.SingleOrDefault(c => c.emailAddress == userUpdated.emailAddress);
        
        if (user == null)
        {
            return NotFound();
        }
        
        if (!string.IsNullOrEmpty(userUpdated.name) && user.name != userUpdated.name)
        {
            user.name = userUpdated.name;
        }

        if (!string.IsNullOrEmpty(userUpdated.lastName) && user.lastName != userUpdated.lastName)
        {
            user.lastName = userUpdated.lastName;
        }

        if (!string.IsNullOrEmpty(userUpdated.streetName) && user.streetName != userUpdated.streetName)
        {
            user.streetName = userUpdated.streetName;
        }

        if (userUpdated.houseNumber.HasValue && user.houseNumber != userUpdated.houseNumber)
        {
            user.houseNumber = userUpdated.houseNumber.Value;
        }

        if (!string.IsNullOrEmpty(userUpdated.voivodeship) && user.voivodeship != userUpdated.voivodeship)
        {
            user.voivodeship = userUpdated.voivodeship;
        }

        if (!string.IsNullOrEmpty(userUpdated.country) && user.country != userUpdated.country)
        {
            user.country = userUpdated.country;
        }

        if (userUpdated.zipcode.HasValue && user.zipcode != userUpdated.zipcode)
        {
            user.zipcode = userUpdated.zipcode.Value;
        }

        if (!string.IsNullOrEmpty(userUpdated.password))
        {
            var md5pass = md5Convert.md5gen(userUpdated.password);
            if (user.password != md5pass)
            {
                user.password = md5pass;
            }
        }

        if (user.phoneNumber != userUpdated.phoneNumber)
        {
            user.phoneNumber = userUpdated.phoneNumber;
        }

       
        _context.User.Update(user);
        await _context.SaveChangesAsync();


        return Ok();
    }
        
    
    // usuwanie konta
    [HttpDelete("account")]
    [Authorize]
    public async Task<IActionResult> DeleteAccount()
    {
        var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized("No valid user session.");
        }

        var user = _context.User.SingleOrDefault(c => c.emailAddress == email);

        if (user == null)
        {
            return NotFound("User not found.");
        }
        
        _context.User.Remove(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Account deleted successfully." });
    }
    
}
