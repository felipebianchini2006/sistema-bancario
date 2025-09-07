using BankApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Controllers;

[ApiController]
[Route("risk")]
public class RiskController : ControllerBase
{
    [HttpGet("score")]
    public IActionResult GetScore([FromQuery] double income, [FromQuery] double balance, [FromQuery] int late = 0)
    {
        var score = RiskInterop.CreditRiskScore(income, balance, late);
        var category = score switch
        {
            <= 0.20 => "baixo",
            <= 0.50 => "moderado",
            <= 0.80 => "alto",
            _ => "crítico"
        };

        return Ok(new
        {
            income,
            balance,
            latePayments = late,
            score = Math.Round(score, 4),
            category
        });
    }
}
