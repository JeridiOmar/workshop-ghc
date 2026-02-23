namespace PomodoroTimer.Api.BusinessObjects
{
    public class DailyBreakdown
    {
        public string Date { get; set; } = string.Empty;
        public int FocusTime { get; set; }
        public int PomodoroCompleted { get; set; }
    }
}
