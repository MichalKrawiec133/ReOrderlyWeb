using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReOrderlyWeb.SQL.Data;
using ReOrderlyWeb.SQL.Data.DAO;
using ReOrderlyWeb.ViewModels;

namespace ReOrderlyWeb.Controllers;


[Authorize]
[ApiController]
public class UserAccountController : ControllerBase
{
    private readonly ReOrderlyWebDbContext _context;

    public UserAccountController(ReOrderlyWebDbContext context)
    {
        _context = context;
    }
    
    
    [HttpGet("account")]
    public IActionResult GetUserData()
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

        var userData = new
        {
            user.name,
            user.lastName,
            user.streetName,
            user.emailAddress,
            user.houseNumber,
            user.voivodeship,
            user.country,
            user.zipcode,
            user.phoneNumber
        };

        return Ok(userData);
    }
    
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
            var md5Pass = md5Convert.md5gen(userCreate.password);
            if (userAdd.password != md5Pass)
            {
                userAdd.password = md5Pass;
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
            var md5Pass = md5Convert.md5gen(userUpdated.password);
            if (user.password != md5Pass)
            {
                user.password = md5Pass;
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
        
    
    [HttpDelete("account")]
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


        var orders = _context.Order
            .Where(o => o.idUser == user.userId)  
            .ToList();


        foreach (var order in orders)
        {

            var orderItems = _context.OrderItems
                .Where(oi => oi.idOrder == order.orderId)
                .ToList();
            
            _context.OrderItems.RemoveRange(orderItems);
            
            _context.Order.Remove(order);
        }
        
        _context.User.Remove(user);
        
        await _context.SaveChangesAsync();

        return Ok(new { message = "Account and associated orders deleted successfully." });
    }


    
    //zmiana hasla
    [HttpPatch("account/changePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
    {
        
        var email = string.IsNullOrEmpty(model.emailAddress) 
            ? User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
            : model.emailAddress;

        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized("No valid user session or email provided.");
        }

        var user = _context.User.SingleOrDefault(c => c.emailAddress == email);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        var oldPasswordHash = md5Convert.md5gen(model.oldPassword);
        if (user.password != oldPasswordHash)
        {
            return BadRequest("Old password is incorrect.");
        }

        if (!string.IsNullOrEmpty(model.newPassword))
        {
            user.password = md5Convert.md5gen(model.newPassword);
            _context.User.Update(user);
            await _context.SaveChangesAsync();
        
            return Ok(new { message = "Password changed successfully." });
        }

        return BadRequest("New password cannot be empty.");
    }

    
    
}
