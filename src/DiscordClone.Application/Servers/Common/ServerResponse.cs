namespace DiscordClone.Application.Servers.Common;

/// <summary>Summary view of a server, returned by create / list / update.</summary>
public record ServerResponse(
    Guid Id,
    string Name,
    string? IconUrl,
    Guid OwnerId,
    string InviteCode,
    DateTime CreatedAt);
