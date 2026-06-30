using DiscordClone.Domain.Common;

namespace DiscordClone.Domain.Entities;

public class Message : BaseEntity
{
    public Guid ChannelId { get; set; }
    public Channel Channel { get; set; } = null!;

    public Guid AuthorId { get; set; }

    public string Content { get; set; } = null!;
    public bool IsEdited { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}
