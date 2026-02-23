using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PomodoroTimer.Api.Interfaces;
using PomodoroTimer.Api.Requests;
using System.Security.Claims;

namespace PomodoroTimer.Api.Controllers;

[ApiController]
[Route("api/settings")]
[Authorize]
public class SettingsController(ISettingsService settingsService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetSettings()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { error = "Invalid user token" });
        }

        var settings = await settingsService.GetSettingsAsync(userId);
        
        if (settings == null)
        {
            return NotFound(new { error = "Settings not found" });
        }

        return Ok(settings);
    }

    [HttpPost]
    public async Task<IActionResult> SaveSettings([FromBody] SettingsRequest request)
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

        var success = await settingsService.SaveSettingsAsync(userId, request);
        
        if (!success)
        {
            return BadRequest(new { error = "Failed to save settings" });
        }

        return Ok(new { message = "Settings saved successfully" });
    }
}
