using ErrorOr;

namespace DiscordClone.Application.Servers.Common;

/// <summary>Centralized server error definitions so codes/messages stay consistent across handlers.</summary>
public static class ServerErrors
{
    public static Error NotFound =>
        Error.NotFound("Server.NotFound", "The server does not exist or you are not a member of it.");

    public static Error Forbidden =>
        Error.Forbidden("Server.Forbidden", "You do not have permission to perform this action on this server.");
}
