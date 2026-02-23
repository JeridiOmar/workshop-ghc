namespace PomodoroTimer.Api.BusinessObjects
{
    public class RankingEntry
    {
        public int Rank { get; set; }
        public string PublicUsername { get; set; } = string.Empty;
        public int TotalFocusTime { get; set; }
        public bool IsCurrentUser { get; set; }
    }
}
