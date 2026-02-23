namespace PomodoroTimer.Api.Models;

public class User
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string PinHash { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
