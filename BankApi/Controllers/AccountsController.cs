using BankApi.Data;
using BankApi.Dto;
using BankApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Controllers;

[ApiController]
[Route("accounts")]
public class AccountsController : ControllerBase
{
    private readonly BankDbContext _db;
    public AccountsController(BankDbContext db) { _db = db; }

    [HttpPost]
    public async Task<ActionResult<AccountView>> Create([FromBody] CreateAccountRequest req)
    {
        // valida duplicidade
        var exists = await _db.Accounts.AnyAsync(a => a.Number == req.Number);
        if (exists) return Conflict("account number exists");

        var acc = new Account { UserId = req.UserId, Number = req.Number, Balance = req.InitialBalance };
        _db.Accounts.Add(acc);
        await _db.SaveChangesAsync();
        return Ok(new AccountView(acc.Id, acc.Number, acc.Balance, acc.UserId));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AccountView>> Get(int id)
    {
        var a = await _db.Accounts.FindAsync(id);
        if (a is null) return NotFound();
        return Ok(new AccountView(a.Id, a.Number, a.Balance, a.UserId));
    }

    [HttpGet("by-user/{userId:int}")]
    public async Task<ActionResult<IEnumerable<AccountView>>> ByUser(int userId)
    {
        var list = await _db.Accounts.Where(a => a.UserId == userId)
            .Select(a => new AccountView(a.Id, a.Number, a.Balance, a.UserId))
            .ToListAsync();
        return Ok(list);
    }
}
