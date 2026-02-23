using PomodoroTimer.Api.Requests;
using PomodoroTimer.Api.Responses;

namespace PomodoroTimer.Api.Interfaces;

public interface ISettingsService
{
    Task<SettingsResponse?> GetSettingsAsync(Guid userId);
    Task<bool> SaveSettingsAsync(Guid userId, SettingsRequest request);
}
