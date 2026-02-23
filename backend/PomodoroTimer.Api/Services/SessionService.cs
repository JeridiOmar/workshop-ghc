using Dapper;
using PomodoroTimer.Api.Data;
using PomodoroTimer.Api.Interfaces;
using PomodoroTimer.Api.Models;
using PomodoroTimer.Api.Requests;
using PomodoroTimer.Api.Responses;

namespace PomodoroTimer.Api.Services;

public class SessionService(IDbConnectionFactory connectionFactory, ILogger<SessionService> logger) : ISessionService
{
    public async Task<int> RecordSessionAsync(Guid userId, SessionRequest request)
    {
        // Validate session type
        var validTypes = new[] { "pomodoro", "short_break", "long_break" };
        if (!validTypes.Contains(request.Type.ToLower()))
        {
            logger.LogWarning("Invalid session type: {Type}", request.Type);
            throw new ArgumentException("Invalid session type");
        }

        // Validate duration
        if (request.DurationSeconds <= 0)
        {
            logger.LogWarning("Invalid duration: {Duration}", request.DurationSeconds);
            throw new ArgumentException("Duration must be positive");
        }

        using var connection = connectionFactory.CreateConnection();

        var sessionId = await connection.ExecuteScalarAsync<int>(
            @"INSERT INTO pomodoro_sessions (user_id, session_type, duration_seconds, is_completed, completed_at)
              VALUES (@UserId, @SessionType, @DurationSeconds, @IsCompleted, @CompletedAt);
              SELECT CAST(SCOPE_IDENTITY() AS INT);",
            new
            {
                UserId = userId,
                SessionType = request.Type.ToLower(),
                request.DurationSeconds,
                request.IsCompleted,
                CompletedAt = request.CompletedAt ?? DateTime.UtcNow
            });

        logger.LogInformation("Session recorded for user {UserId}: {Type} - {Duration}min", 
            userId, request.Type, request.DurationSeconds);

        return sessionId;
    }

    public async Task<List<SessionResponse>> GetSessionsAsync(Guid userId, DateTime? startDate, DateTime? endDate, int? limit)
    {
        using var connection = connectionFactory.CreateConnection();

        var sql = @"SELECT TOP(@Limit) id, user_id, session_type As SessionType, duration_seconds As DurationSeconds, is_completed As IsCompleted, completed_at As CompletedAt
                    FROM pomodoro_sessions
                    WHERE user_id = @UserId";

        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId);
        parameters.Add("Limit", limit ?? int.MaxValue);

        if (startDate.HasValue)
        {
            sql += " AND completed_at >= @StartDate";
            parameters.Add("StartDate", startDate.Value);
        }

        if (endDate.HasValue)
        {
            sql += " AND completed_at <= @EndDate";
            parameters.Add("EndDate", endDate.Value);
        }

        sql += " ORDER BY completed_at DESC";

        var sessions = await connection.QueryAsync<PomodoroSession>(sql, parameters);

        return [.. sessions.Select(s => new SessionResponse
        {
            Type = s.SessionType,
            DurationSeconds = s.DurationSeconds,
            IsCompleted = s.IsCompleted,
            CompletedAt = s.CompletedAt
        })];
    }
}
