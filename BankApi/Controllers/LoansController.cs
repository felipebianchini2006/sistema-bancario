using BankApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Controllers;

public record LoanAssessRequest(double Income, double Balance, int LatePayments, double Requested);

[ApiController]
[Route("loans")]
public class LoansController : ControllerBase
{
    private readonly RiskService _risk;
    public LoansController(RiskService risk) => _risk = risk;

    [HttpPost("assess")]
    [Authorize] // opcional: exigir JWT
    public IActionResult Assess([FromBody] LoanAssessRequest req)
    {
        var (score, category) = _risk.Assess(req.Income, req.Balance, req.LatePayments);

        // política simples
        var approved = score <= 0.50 && req.Requested <= req.Income * 0.3;
        var maxAmount = score <= 0.20 ? req.Income * 0.5
                     : score <= 0.50 ? req.Income * 0.3
                     : 0;

        return Ok(new
        {
            req.Income,
            req.Balance,
            req.LatePayments,
            req.Requested,
            score,
            category,
            approved,
            maxAmount = Math.Round(maxAmount, 2),
            suggestedApr = score <= 0.20 ? 0.18 : score <= 0.50 ? 0.28 : null as double?
        });
    }
}
