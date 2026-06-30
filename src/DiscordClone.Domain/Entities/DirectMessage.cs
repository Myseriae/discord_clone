using DiscordClone.Domain.Common;

namespace DiscordClone.Domain.Entities;

public class DirectMessage : BaseEntity
{
    public Guid DMChannelId { get; set; }
    public DirectMessageChannel DMChannel { get; set; } = null!;

    public Guid AuthorId { get; set; }

    public string Content { get; set; } = null!;
    public bool IsEdited { get; set; }
    public bool IsDeleted { get; set; }
}
