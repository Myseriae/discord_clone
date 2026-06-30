namespace DiscordClone.Application.Servers.Common;

public record ChannelResponse(
    Guid Id,
    string Name,
    string Type,
    int Position,
    Guid? CategoryId);
