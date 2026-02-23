namespace PomodoroTimer.Api.BusinessObjects
{
    public class AllTimeStats
    {
        public int TotalFocusTime { get; set; }
        public int TotalPomodoro { get; set; }
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public int AccountAge { get; set; }
    }
}
