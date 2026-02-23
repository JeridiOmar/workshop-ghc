namespace PomodoroTimer.Api.Responses
{
    public class AuthResponse
    {
        public Guid UserId { get; set; }
        public required string Username { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("bearerToken")]
        public required string BearerToken { get; set; }
    }
}
