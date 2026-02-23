using PomodoroTimer.Api.Requests;
using PomodoroTimer.Api.Responses;

namespace PomodoroTimer.Api.Interfaces;

public interface IAuthService
{
    Task<AuthResponse?> RegisterAsync(RegisterRequest request);

    Task<AuthResponse?> LoginAsync(LoginRequest request);

    string GenerateJwtToken(Guid userId, string username);
}