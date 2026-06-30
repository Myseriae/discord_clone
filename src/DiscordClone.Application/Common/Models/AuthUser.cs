namespace DiscordClone.Application.Common.Models;

/// <summary>
/// A lightweight projection of an authenticated user passed across the auth boundary.
/// Keeps the Application layer free of ASP.NET Identity types.
/// </summary>
public record AuthUser(Guid Id, string Email, string DisplayName);
