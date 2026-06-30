using DiscordClone.Domain.Common;
using DiscordClone.Domain.Enums;

namespace DiscordClone.Domain.Entities;

public class Channel : BaseEntity
{
    public Guid ServerId { get; set; }
    public Server Server { get; set; } = null!;

    public string Name { get; set; } = null!;
    public ChannelType Type { get; set; }
    public int Position { get; set; }

    /// <summary>Parent category channel (a channel with <see cref="ChannelType.Category"/>), or null for top-level.</summary>
    public Guid? CategoryId { get; set; }
    public Channel? Category { get; set; }
    public ICollection<Channel> ChildChannels { get; set; } = new List<Channel>();

    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<ChannelPermission> Permissions { get; set; } = new List<ChannelPermission>();
}
