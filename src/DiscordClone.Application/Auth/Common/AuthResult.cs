namespace DiscordClone.Application.Auth.Common;

/// <summary>The payload returned by register / login / refresh: the user plus a fresh token pair.</summary>
public record AuthResult(
    Guid UserId,
    string Email,
    string DisplayName,
    string AccessToken,
    DateTime AccessTokenExpiresAt,
    string RefreshToken);
