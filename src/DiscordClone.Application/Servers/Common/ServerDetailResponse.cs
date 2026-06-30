namespace DiscordClone.Application.Servers.Common;

/// <summary>Detailed view of a server including its channels, returned by get-by-id.</summary>
public record ServerDetailResponse(
    Guid Id,
    string Name,
    string? IconUrl,
    Guid OwnerId,
    string InviteCode,
    DateTime CreatedAt,
    IReadOnlyList<ChannelResponse> Channels);
