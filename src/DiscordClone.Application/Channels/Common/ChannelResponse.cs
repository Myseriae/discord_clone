namespace DiscordClone.Application.Channels.Common;

public record ChannelResponse(
    Guid Id,
    string Name,
    string Type,
    int Position,
    Guid? CategoryId);
