namespace PomodoroTimer.Api.Models;

public class PomodoroSession
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public required string SessionType { get; set; } // 'pomodoro', 'short_break', 'long_break'
    public int DurationSeconds { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
}
