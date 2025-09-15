using BankApi.Data;
using BankApi.Models;
using System;

namespace BankApi.Services;

public sealed class LoanDecisionService
{
    private readonly BankDbContext _db;
    private readonly RiskService _risk;

    public LoanDecisionService(BankDbContext db, RiskService risk)
    { _db = db; _risk = risk; }

    public async Task<LoanApplication> AssessAndSaveAsync(
        double income, double balance, int late, double requested, string? userId, int? accountId)
    {
        var (score, category) = _risk.Assess(income, balance, late);

        var approved = score <= 0.50 && requested <= income * 0.3;
        var maxAmount = score <= 0.20 ? income * 0.5
                     : score <= 0.50 ? income * 0.3
                     : 0;
        double? apr = score <= 0.20 ? 0.18 : score <= 0.50 ? 0.28 : null;

        var app = new LoanApplication
        {
            AccountId = accountId,
            Income = income,
            Balance = balance,
            LatePayments = late,
            Requested = requested,
            Score = score,
            Category = category,
            Approved = approved,
            MaxAmount = Math.Round(maxAmount, 2),
            SuggestedApr = apr,
            UserId = userId
        };

        _db.LoanApplications.Add(app);
        await _db.SaveChangesAsync();
        return app;
    }
}
