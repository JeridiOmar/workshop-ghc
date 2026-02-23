namespace PomodoroTimer.Api.BusinessObjects
{
    public class WeeklyStats
    {
        public int FocusTime { get; set; }
        public int PomodoroCompleted { get; set; }
        public List<DailyBreakdown> DailyBreakdown { get; set; } = [];
    }
}