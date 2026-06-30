using DiscordClone.Domain.Common;

namespace DiscordClone.Domain.Entities;

public class DirectMessageChannel : BaseEntity
{
    public ICollection<DMParticipant> Participants { get; set; } = new List<DMParticipant>();
    public ICollection<DirectMessage> Messages { get; set; } = new List<DirectMessage>();
}
