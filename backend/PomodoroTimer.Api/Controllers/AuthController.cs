using Microsoft.AspNetCore.Mvc;
using PomodoroTimer.Api.Interfaces;
using PomodoroTimer.Api.Requests;

namespace PomodoroTimer.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { error = "Invalid request data" });
        }

        var response = await authService.RegisterAsync(request);

        if (response == null)
        {
            return BadRequest(new { error = "Username already exists or invalid data" });
        }

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { error = "Invalid request data" });
        }

        var response = await authService.LoginAsync(request);

        if (response == null)
        {
            return Unauthorized(new { error = "Invalid username or PIN" });
        }

        return Ok(response);
    }
}