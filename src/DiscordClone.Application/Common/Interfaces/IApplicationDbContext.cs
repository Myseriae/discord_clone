using DiscordClone.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Application.Common.Interfaces;

/// <summary>
/// Abstraction over the persistence context exposed to the Application layer.
/// Implemented by the Infrastructure AppDbContext. Identity types (ApplicationUser, RefreshToken)
/// are intentionally not exposed here — they are an Infrastructure concern.
/// </summary>
public interface IApplicationDbContext
{
    DbSet<Server> Servers { get; }
    DbSet<ServerMember> ServerMembers { get; }
    DbSet<ServerRole> ServerRoles { get; }
    DbSet<ServerMemberRole> ServerMemberRoles { get; }
    DbSet<Channel> Channels { get; }
    DbSet<ChannelPermission> ChannelPermissions { get; }
    DbSet<Message> Messages { get; }
    DbSet<Attachment> Attachments { get; }
    DbSet<DirectMessageChannel> DirectMessageChannels { get; }
    DbSet<DMParticipant> DMParticipants { get; }
    DbSet<DirectMessage> DirectMessages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
