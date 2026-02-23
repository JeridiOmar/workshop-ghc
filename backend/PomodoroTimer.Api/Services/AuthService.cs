using Dapper;
using Microsoft.IdentityModel.Tokens;
using PomodoroTimer.Api.Data;
using PomodoroTimer.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using PomodoroTimer.Api.Interfaces;
using PomodoroTimer.Api.Responses;
using PomodoroTimer.Api.Requests;

namespace PomodoroTimer.Api.Services;

public partial class AuthService(IDbConnectionFactory connectionFactory, IConfiguration configuration, ILogger<AuthService> logger) : IAuthService
{
    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        // Validate username
        if (string.IsNullOrWhiteSpace(request.Username) || request.Username.Length < 3 || request.Username.Length > 50)
        {
            logger.LogWarning("Invalid username length: {Length}", request.Username?.Length);
            return null;
        }

        // Validate PIN format (must be exactly 6 digits)
        if (!MyRegex().IsMatch(request.Pin))
        {
            logger.LogWarning("Invalid PIN format");
            return null;
        }

        using var connection = connectionFactory.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            // Check if username already exists
            var existingUser = await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT id, username, pin_hash, created_at FROM users WHERE username = @Username",
                new { request.Username },
                transaction);

            if (existingUser != null)
            {
                logger.LogWarning("Username already exists: {Username}", request.Username);
                return null;
            }

            // Hash the PIN
            var pinHash = BCrypt.Net.BCrypt.HashPassword(request.Pin);

            // Create new user
            var userId = Guid.NewGuid();
            await connection.ExecuteAsync(
                "INSERT INTO users (id, username, pin_hash, created_at) VALUES (@Id, @Username, @PinHash, @CreatedAt)",
                new { Id = userId, request.Username, PinHash = pinHash, CreatedAt = DateTime.UtcNow },
                transaction);

            // Create default settings for the user
            await connection.ExecuteAsync(
                @"INSERT INTO user_settings (user_id, pomodoro_duration, short_break_duration, long_break_duration, 
                    notifications_enabled, notification_sound, notification_volume, theme_focus, theme_short_break, 
                    theme_long_break, auto_start_breaks, auto_start_pomodoro, long_break_interval, updated_at) 
                  VALUES (@UserId, 25, 5, 15, 1, 'bell', 80, '#BA4949', '#38858A', '#397097', 0, 0, 4, @UpdatedAt)",
                new { UserId = userId, UpdatedAt = DateTime.UtcNow },
                transaction);

            transaction.Commit();

            logger.LogInformation("User registered successfully: {Username}", request.Username);

            // Generate JWT token
            var token = GenerateJwtToken(userId, request.Username);

            return new AuthResponse
            {
                UserId = userId,
                Username = request.Username,
                BearerToken = token
            };
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            logger.LogError(ex, "Error during user registration");
            return null;
        }
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        using var connection = connectionFactory.CreateConnection();

        // Find user by username
        var user = await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT id, username, pin_hash AS PinHash, created_at FROM users WHERE username = @Username",
            new { request.Username });

        if (user == null)
        {
            logger.LogWarning("User not found: {Username}", request.Username);
            return null;
        }

        // Verify PIN
        if (!BCrypt.Net.BCrypt.Verify(request.Pin, user.PinHash))
        {
            logger.LogWarning("Invalid PIN for user: {Username}", request.Username);
            return null;
        }

        logger.LogInformation("User logged in successfully: {Username}", user.Username);

        // Generate JWT token
        var token = GenerateJwtToken(user.Id, user.Username);

        return new AuthResponse
        {
            UserId = user.Id,
            Username = user.Username,
            BearerToken = token
        };
    }

    public string GenerateJwtToken(Guid userId, string username)
    {
        var jwtSecret = configuration["JwtSecret"] ?? throw new InvalidOperationException("JwtSecret not configured");
        var key = Encoding.ASCII.GetBytes(jwtSecret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username)
            ]),
            Expires = DateTime.UtcNow.AddDays(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    [GeneratedRegex(@"^\d{6}$")]
    private static partial Regex MyRegex();
}
