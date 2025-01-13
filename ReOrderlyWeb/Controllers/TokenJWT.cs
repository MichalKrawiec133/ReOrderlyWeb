using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ReOrderlyWeb.SQL.Data.DAO;

namespace ReOrderlyWeb.Controllers;

public class TokenJWT
{
    public static string GenerateToken(User user)
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
            expires: DateTime.Now.AddSeconds(30000),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}