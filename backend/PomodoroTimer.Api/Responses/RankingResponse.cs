using PomodoroTimer.Api.BusinessObjects;

namespace PomodoroTimer.Api.Responses
{
    public class RankingResponse
    {
        public List<RankingEntry> Rankings { get; set; } = [];
        public Pagination Pagination { get; set; } = new();
    }
}