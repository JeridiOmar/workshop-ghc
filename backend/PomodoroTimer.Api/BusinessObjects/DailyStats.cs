namespace PomodoroTimer.Api.BusinessObjects
{
    public class DailyStats
    {
        public int FocusTime { get; set; }
        public int PomodoroCompleted { get; set; }
        public string Date { get; set; } = string.Empty;
    }
}
