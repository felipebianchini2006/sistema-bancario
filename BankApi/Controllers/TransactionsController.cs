using BankApi.Data;
using BankApi.Dto;
using BankApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Controllers;

[ApiController]
[Route("transactions")]
public class TransactionsController : ControllerBase
{
    private readonly BankDbContext _db;
    public TransactionsController(BankDbContext db) { _db = db; }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateTransactionRequest req)
    {
        var acc = await _db.Accounts.FindAsync(req.AccountId);
        if (acc is null) return NotFound("account");
        if (req.Type == TxType.debit && acc.Balance < req.Amount) return BadRequest("insufficient funds");

        var tx = new TransactionRecord
        {
            AccountId = req.AccountId,
            Type = req.Type,
            Amount = req.Amount,
            Description = req.Description
        };
        _db.Transactions.Add(tx);
        acc.Balance += req.Type == TxType.credit ? req.Amount : -req.Amount;
        await _db.SaveChangesAsync();
        return Ok(tx.Id);
    }

    [HttpPost("transfer")]
    public async Task<ActionResult> Transfer([FromBody] TransferRequest req)
    {
        if (req.FromAccountId == req.ToAccountId) return BadRequest("same account");

        var from = await _db.Accounts.FindAsync(req.FromAccountId);
        var to = await _db.Accounts.FindAsync(req.ToAccountId);
        if (from is null || to is null) return NotFound("account");
        if (from.Balance < req.Amount) return BadRequest("insufficient funds");

        using var t = await _db.Database.BeginTransactionAsync();
        try
        {
            from.Balance -= req.Amount;
            to.Balance += req.Amount;
            _db.Transactions.Add(new TransactionRecord { AccountId = from.Id, Type = TxType.debit, Amount = req.Amount, Description = req.Description ?? "transfer out" });
            _db.Transactions.Add(new TransactionRecord { AccountId = to.Id, Type = TxType.credit, Amount = req.Amount, Description = req.Description ?? "transfer in" });
            await _db.SaveChangesAsync();
            await t.CommitAsync();
            return Ok("ok");
        }
        catch { await t.RollbackAsync(); throw; }
    }

    [HttpGet("by-account/{accountId:int}")]
    public async Task<ActionResult<IEnumerable<TransactionRecord>>> ByAccount(int accountId)
    {
        var list = await _db.Transactions
            .Where(x => x.AccountId == accountId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
        return Ok(list);
    }
}
