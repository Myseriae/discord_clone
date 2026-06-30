using DiscordClone.Domain.Common;

namespace DiscordClone.Domain.Entities;

/// <summary>Per-channel permission override for a role, layered on top of the role's base permissions.</summary>
public class ChannelPermission : BaseEntity
{
    public Guid ChannelId { get; set; }
    public Channel Channel { get; set; } = null!;

    public Guid RoleId { get; set; }
    public ServerRole Role { get; set; } = null!;

    public long Allow { get; set; }
    public long Deny { get; set; }
}
