using DiscordClone.Domain.Common;

namespace DiscordClone.Domain.Entities;

public class DMParticipant : BaseEntity
{
    public Guid DMChannelId { get; set; }
    public DirectMessageChannel DMChannel { get; set; } = null!;

    public Guid UserId { get; set; }
}
