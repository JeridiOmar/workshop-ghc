using Microsoft.AspNetCore.Mvc;

namespace PomodoroTimer.Api.Controllers;

[ApiController]
[Route("api/stats")]
public class StatsController() : ControllerBase
{
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpGet("ranking")]
    public async Task<IActionResult> GetRanking([FromQuery] int page = 1, [FromQuery] int limit = 50)
    {
        await Task.CompletedTask;
        return Ok();
    }
}
