using DiscordClone.Domain.Common;

namespace DiscordClone.Domain.Entities;

/// <summary>Join entity giving a server member many roles (Discord-style multi-role membership).</summary>
public class ServerMemberRole : BaseEntity
{
    public Guid ServerMemberId { get; set; }
    public ServerMember ServerMember { get; set; } = null!;

    public Guid ServerRoleId { get; set; }
    public ServerRole ServerRole { get; set; } = null!;
}
