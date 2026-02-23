namespace PomodoroTimer.Api.Responses
{
    public class SettingsResponse
    {
        public string? PublicUsername { get; set; }
        public int PomodoroDuration { get; set; }
        public int ShortBreakDuration { get; set; }
        public int LongBreakDuration { get; set; }
        public bool NotificationsEnabled { get; set; }
        public string NotificationSound { get; set; } = "bell";
        public int NotificationVolume { get; set; }
        public string ThemeFocus { get; set; } = "#BA4949";
        public string ThemeShortBreak { get; set; } = "#38858A";
        public string ThemeLongBreak { get; set; } = "#397097";
        public bool AutoStartBreaks { get; set; }
        public bool AutoStartPomodoro { get; set; }
        public int LongBreakInterval { get; set; }
    }
}
