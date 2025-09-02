using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BankApi.Services;

public class JwtService
{
    private readonly IConfiguration _cfg;
    public JwtService(IConfiguration cfg) => _cfg = cfg;

    public string Create(string email, string role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _cfg["Jwt:Issuer"],
            audience: _cfg["Jwt:Audience"],
            claims: new[] { new Claim(ClaimTypes.Email, email), new Claim(ClaimTypes.Role, role) },
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
