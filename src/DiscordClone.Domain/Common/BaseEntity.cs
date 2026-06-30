namespace DiscordClone.Domain.Common;

/// <summary>Base type for all persisted domain entities. Guid PKs are required project-wide.</summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
}
