using Microsoft.AspNetCore.Mvc;

namespace BankApi.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping() => Ok(new { status = "ok", ts = DateTime.UtcNow });
}
