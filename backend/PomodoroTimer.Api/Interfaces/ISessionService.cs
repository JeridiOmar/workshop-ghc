using PomodoroTimer.Api.Requests;
using PomodoroTimer.Api.Responses;

namespace PomodoroTimer.Api.Interfaces;

public interface ISessionService
{
    Task<int> RecordSessionAsync(Guid userId, SessionRequest request);
    Task<List<SessionResponse>> GetSessionsAsync(Guid userId, DateTime? startDate, DateTime? endDate, int? limit);
}
