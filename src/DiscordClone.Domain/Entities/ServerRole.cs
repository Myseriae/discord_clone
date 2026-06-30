using DiscordClone.Domain.Common;

namespace DiscordClone.Domain.Entities;

public class ServerRole : BaseEntity
{
    public Guid ServerId { get; set; }
    public Server Server { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string? Color { get; set; }
    public int Position { get; set; }

    /// <summary>
    /// Marks the server's implicit "@everyone" role. Its permissions form the baseline applied to
    /// every member regardless of explicit role assignments. Exactly one default role per server.
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>OR'd bitfield of this role's base permissions. See <see cref="Constants.Permissions"/>.</summary>
    public long Permissions { get; set; }

    public ICollection<ServerMemberRole> MemberRoles { get; set; } = new List<ServerMemberRole>();
    public ICollection<ChannelPermission> ChannelPermissions { get; set; } = new List<ChannelPermission>();
}
