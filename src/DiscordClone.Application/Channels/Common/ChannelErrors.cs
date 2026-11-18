using ErrorOr;

namespace DiscordClone.Application.Channels.Common;

/// <summary>Centralized channel error definitions, mirroring ServerErrors.</summary>
public static class ChannelErrors
{
    public static Error NotFound =>
        Error.NotFound("Channel.NotFound", "The channel does not exist or you are not a member of its server.");

    public static Error Forbidden =>
        Error.Forbidden("Channel.Forbidden", "You do not have permission to manage channels in this server.");

    public static Error InvalidType =>
        Error.Validation("Channel.InvalidType", "Channel type must be one of: Text, Voice, Announcement.");
}
