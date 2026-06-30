namespace DiscordClone.Application.Common.Models;

/// <summary>A freshly issued refresh token paired with its UTC expiry.</summary>
public record RefreshTokenResult(string Token, DateTime ExpiresAtUtc);
