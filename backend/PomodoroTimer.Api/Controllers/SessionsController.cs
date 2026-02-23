using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PomodoroTimer.Api.Interfaces;
using PomodoroTimer.Api.Requests;
using System.Security.Claims;

namespace PomodoroTimer.Api.Controllers;

[ApiController]
[Route("api/sessions")]
[Authorize]
public class SessionsController(ISessionService sessionService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> RecordSession([FromBody] SessionRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { error = "Invalid request data" });
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { error = "Invalid user token" });
        }

        try
        {
            var sessionId = await sessionService.RecordSessionAsync(userId, request);
            return Ok(new { sessionId, message = "Session recorded successfully" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetSessions(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int? limit)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { error = "Invalid user token" });
        }

        var sessions = await sessionService.GetSessionsAsync(userId, startDate, endDate, limit);
        return Ok(sessions);
    }
}
