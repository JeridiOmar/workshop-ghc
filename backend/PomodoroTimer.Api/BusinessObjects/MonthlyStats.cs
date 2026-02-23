namespace PomodoroTimer.Api.BusinessObjects
{
    public class MonthlyStats
    {
        public int FocusTime { get; set; }
        public int PomodoroCompleted { get; set; }
        public List<CalendarDay> Calendar { get; set; } = [];
    }
}
