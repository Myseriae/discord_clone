using DiscordClone.Domain.Common;

namespace DiscordClone.Domain.Entities;

public class ServerMember : BaseEntity
{
    public Guid ServerId { get; set; }
    public Server Server { get; set; } = null!;

    public Guid UserId { get; set; }

    public DateTime JoinedAt { get; set; }

    public ICollection<ServerMemberRole> MemberRoles { get; set; } = new List<ServerMemberRole>();
}
