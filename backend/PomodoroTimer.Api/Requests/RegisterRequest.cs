namespace PomodoroTimer.Api.Requests
{
    public class RegisterRequest
    {
        public required string Username { get; set; }
        public required string Pin { get; set; }
    }
}
