namespace PomodoroTimer.Api.Requests
{
    public class SessionRequest
    {
        public required string Type { get; set; } // 'pomodoro', 'short_break', 'long_break'
        public int DurationSeconds { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
