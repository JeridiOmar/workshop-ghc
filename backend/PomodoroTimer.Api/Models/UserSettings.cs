namespace PomodoroTimer.Api.Models;

public class UserSettings
{
    public Guid UserId { get; set; }
    public string? PublicUsername { get; set; }
    public int PomodoroDuration { get; set; } = 25;
    public int ShortBreakDuration { get; set; } = 5;
    public int LongBreakDuration { get; set; } = 15;
    public bool NotificationsEnabled { get; set; } = true;
    public string NotificationSound { get; set; } = "bell";
    public int NotificationVolume { get; set; } = 80;
    public string ThemeFocus { get; set; } = "#BA4949";
    public string ThemeShortBreak { get; set; } = "#38858A";
    public string ThemeLongBreak { get; set; } = "#397097";
    public bool AutoStartBreaks { get; set; } = false;
    public bool AutoStartPomodoro { get; set; } = false;
    public int LongBreakInterval { get; set; } = 4;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
