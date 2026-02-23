using Dapper;
using PomodoroTimer.Api.Data;
using PomodoroTimer.Api.Interfaces;
using PomodoroTimer.Api.Models;
using PomodoroTimer.Api.Requests;
using PomodoroTimer.Api.Responses;

namespace PomodoroTimer.Api.Services;

public class SettingsService(IDbConnectionFactory connectionFactory, ILogger<SettingsService> logger) : ISettingsService
{
    public async Task<SettingsResponse?> GetSettingsAsync(Guid userId)
    {
        using var connection = connectionFactory.CreateConnection();

        var settings = await connection.QueryFirstOrDefaultAsync<UserSettings>(
            @"SELECT user_id, public_username AS PublicUsername, pomodoro_duration AS PomodoroDuration, short_break_duration AS ShortBreakDuration, 
                     long_break_duration AS LongBreakDuration, notifications_enabled AS NotificationsEnabled, notification_sound AS NotificationSound, 
                     notification_volume AS NotificationVolume, theme_focus AS ThemeFocus, theme_short_break AS ThemeShortBreak,
                     theme_long_break AS ThemeLongBreak, auto_start_breaks AS AutoStartBreaks, auto_start_pomodoro AS AutoStartPomodoro, 
                     long_break_interval AS LongBreakInterval, updated_at AS UpdatedAt
              FROM user_settings WHERE user_id = @UserId",
            new { UserId = userId });
        
        if (settings == null)
        {
            logger.LogWarning("Settings not found for user: {UserId}", userId);
            return null;
        }

        return new SettingsResponse
        {
            PublicUsername = settings.PublicUsername,
            PomodoroDuration = settings.PomodoroDuration,
            ShortBreakDuration = settings.ShortBreakDuration,
            LongBreakDuration = settings.LongBreakDuration,
            NotificationsEnabled = settings.NotificationsEnabled,
            NotificationSound = settings.NotificationSound,
            NotificationVolume = settings.NotificationVolume,
            ThemeFocus = settings.ThemeFocus,
            ThemeShortBreak = settings.ThemeShortBreak,
            ThemeLongBreak = settings.ThemeLongBreak,
            AutoStartBreaks = settings.AutoStartBreaks,
            AutoStartPomodoro = settings.AutoStartPomodoro,
            LongBreakInterval = settings.LongBreakInterval
        };
    }

    public async Task<bool> SaveSettingsAsync(Guid userId, SettingsRequest request)
    {
        using var connection = connectionFactory.CreateConnection();

        // Check if settings exist
        var exists = await connection.ExecuteScalarAsync<bool>(
            "SELECT CAST(CASE WHEN EXISTS(SELECT 1 FROM user_settings WHERE user_id = @UserId) THEN 1 ELSE 0 END AS BIT)",
            new { UserId = userId });
        
        if (!exists)
        {
            logger.LogWarning("Settings not found for user: {UserId}", userId);
            return false;
        }

        // Update settings
        var rowsAffected = await connection.ExecuteAsync(
            @"UPDATE user_settings SET 
                public_username = @PublicUsername,
                pomodoro_duration = @PomodoroDuration,
                short_break_duration = @ShortBreakDuration,
                long_break_duration = @LongBreakDuration,
                notifications_enabled = @NotificationsEnabled,
                notification_sound = @NotificationSound,
                notification_volume = @NotificationVolume,
                theme_focus = @ThemeFocus,
                theme_short_break = @ThemeShortBreak,
                theme_long_break = @ThemeLongBreak,
                auto_start_breaks = @AutoStartBreaks,
                auto_start_pomodoro = @AutoStartPomodoro,
                long_break_interval = @LongBreakInterval,
                updated_at = @UpdatedAt
              WHERE user_id = @UserId",
            new
            {
                UserId = userId,
                request.PublicUsername,
                request.PomodoroDuration,
                request.ShortBreakDuration,
                request.LongBreakDuration,
                request.NotificationsEnabled,
                request.NotificationSound,
                request.NotificationVolume,
                request.ThemeFocus,
                request.ThemeShortBreak,
                request.ThemeLongBreak,
                request.AutoStartBreaks,
                request.AutoStartPomodoro,
                request.LongBreakInterval,
                UpdatedAt = DateTime.UtcNow
            });

        logger.LogInformation("Settings saved for user: {UserId}", userId);
        return rowsAffected > 0;
    }
}
