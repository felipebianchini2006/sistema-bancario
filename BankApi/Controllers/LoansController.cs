using BankApi.Data;
using BankApi.Models;
using BankApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace BankApi.Controllers;

public record LoanAssessRequest(double Income, double Balance, int LatePayments, double Requested, int? AccountId);

[ApiController]
[Route("loans")]
public class LoansController : ControllerBase
{
    private readonly LoanDecisionService _svc;
    private readonly BankDbContext _db;

    public LoansController(LoanDecisionService svc, BankDbContext db)
    {
        _svc = svc;
        _db = db;
    }

    [HttpPost("assess")]
    [Authorize]
    public async Task<IActionResult> Assess([FromBody] LoanAssessRequest req)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var app = await _svc.AssessAndSaveAsync(
            req.Income, req.Balance, req.LatePayments, req.Requested, userId, req.AccountId
        );
        return Ok(app);
    }

    [HttpGet("history")]
    [Authorize]
    public async Task<IActionResult> History([FromQuery] int? accountId, [FromQuery] int take = 50, [FromQuery] int skip = 0)
    {
        var q = _db.LoanApplications.AsNoTracking().OrderByDescending(x => x.Id);
        if (accountId.HasValue) q = q.Where(x => x.AccountId == accountId.Value).OrderByDescending(x => x.Id);
        var items = await q.Skip(skip).Take(Math.Clamp(take, 1, 200)).ToListAsync();
        return Ok(items);
    }
    public record LoanReportQuery(DateTime? From, DateTime? To, int? AccountId, string? UserId);

    [HttpGet("report")]
    [Authorize]
    public async Task<IActionResult> Report([FromQuery] LoanReportQuery q)
    {
        var from = q.From?.ToUniversalTime();
        var to = q.To?.ToUniversalTime();

        IQueryable<LoanApplication> baseQ = _db.LoanApplications.AsNoTracking();

        if (from.HasValue) baseQ = baseQ.Where(x => x.CreatedAt >= from.Value);
        if (to.HasValue) baseQ = baseQ.Where(x => x.CreatedAt < to.Value);
        if (q.AccountId.HasValue) baseQ = baseQ.Where(x => x.AccountId == q.AccountId);
        if (!string.IsNullOrWhiteSpace(q.UserId)) baseQ = baseQ.Where(x => x.UserId == q.UserId);

        var items = await baseQ.ToListAsync();
        if (items.Count == 0)
            return Ok(new
            {
                total = 0,
                approved = 0,
                approvalRate = 0.0,
                avgRequested = 0.0,
                avgScore = 0.0,
                sumRequested = 0.0,
                byCategory = Array.Empty<object>()
            });

        double Avg(Func<LoanApplication, double> f) => Math.Round(items.Average(f), 4);
        double Sum(Func<LoanApplication, double> f) => Math.Round(items.Sum(f), 2);

        var total = items.Count;
        var approved = items.Count(x => x.Approved);
        var approvalRate = Math.Round(approved * 100.0 / total, 2);

        var byCategory = items
            .GroupBy(x => x.Category)
            .Select(g => new {
                category = g.Key,
                total = g.Count(),
                approved = g.Count(x => x.Approved),
                approvalRate = Math.Round(g.Count(x => x.Approved) * 100.0 / g.Count(), 2),
                avgScore = Math.Round(g.Average(x => x.Score), 4),
                avgRequested = Math.Round(g.Average(x => x.Requested), 2),
                sumRequested = Math.Round(g.Sum(x => x.Requested), 2),
            })
            .OrderByDescending(x => x.total)
            .ToList();

        return Ok(new
        {
            total,
            approved,
            approvalRate,
            avgRequested = Avg(x => x.Requested),
            avgScore = Avg(x => x.Score),
            sumRequested = Sum(x => x.Requested),
            byCategory
        });
    }
}

