using ErrorOr;

namespace DiscordClone.Application.Auth.Common;

/// <summary>Centralized auth error definitions so codes/messages stay consistent across layers.</summary>
public static class AuthErrors
{
    public static Error DuplicateEmail =>
        Error.Conflict("Auth.DuplicateEmail", "A user with this email already exists.");

    public static Error InvalidCredentials =>
        Error.Unauthorized("Auth.InvalidCredentials", "Invalid email or password.");

    public static Error InvalidRefreshToken =>
        Error.Unauthorized("Auth.InvalidRefreshToken", "The refresh token is invalid, expired, or already used.");

    public static Error UserNotFound =>
        Error.NotFound("Auth.UserNotFound", "The user no longer exists.");
}
