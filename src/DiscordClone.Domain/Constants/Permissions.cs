namespace DiscordClone.Domain.Constants;

/// <summary>
/// Permission flags stored as an OR'd bigint bitfield.
/// <see cref="DiscordClone.Domain.Entities.ServerRole"/> holds a role's base permissions;
/// <see cref="DiscordClone.Domain.Entities.ChannelPermission"/> layers per-channel Allow/Deny overrides on top.
/// </summary>
public static class Permissions
{
    public const long None           = 0L;
    public const long ViewChannels   = 1L << 0;
    public const long SendMessages   = 1L << 1;
    public const long ManageChannels = 1L << 2;
    public const long ManageRoles    = 1L << 3;
    public const long KickMembers    = 1L << 4;
    public const long MoveMembers    = 1L << 5;
    public const long ManageServer   = 1L << 6;
    public const long BanMembers     = 1L << 7;
}
