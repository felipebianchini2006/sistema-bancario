using BankApi.Data;
using BankApi.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly BankDbContext _db;
    private readonly BankApi.Services.JwtService _jwt;

    public AuthController(BankDbContext db, BankApi.Services.JwtService jwt)
    { _db = db; _jwt = jwt; }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest req)
    {
        var u = await _db.Users.FirstOrDefaultAsync(x => x.Email == req.Email);
        if (u is null || !BCrypt.Net.BCrypt.Verify(req.Password, u.PasswordHash))
            return Unauthorized();

        var token = _jwt.Create(u.Email, u.Role);
        return Ok(new LoginResponse(token));
    }
}
