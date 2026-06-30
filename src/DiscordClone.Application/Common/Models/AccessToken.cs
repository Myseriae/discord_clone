namespace DiscordClone.Application.Common.Models;

/// <summary>A signed JWT access token paired with its UTC expiry.</summary>
public record AccessToken(string Value, DateTime ExpiresAtUtc);
