using DiscordClone.Domain.Common;
using DiscordClone.Domain.Constants;
using DiscordClone.Domain.Enums;

namespace DiscordClone.Domain.Entities;

public class Server : BaseEntity
{
    public Guid OwnerId { get; set; }
    public string Name { get; set; } = null!;
    public string? IconUrl { get; set; }
    public string InviteCode { get; set; } = null!;

    public ICollection<Channel> Channels { get; set; } = new List<Channel>();
    public ICollection<ServerMember> Members { get; set; } = new List<ServerMember>();
    public ICollection<ServerRole> Roles { get; set; } = new List<ServerRole>();

    /// <summary>The default permissions granted to every member through the "@everyone" role.</summary>
    public const long DefaultEveryonePermissions = Permissions.ViewChannels | Permissions.SendMessages;

    /// <summary>
    /// Creates a new server fully seeded with its invariant starting state: the owner as the first
    /// member, an "@everyone" default role, and a "general" text channel. The whole graph is built
    /// via navigation collections so EF Core persists it in a single transaction.
    /// </summary>
    public static Server Create(Guid ownerId, string name, string? iconUrl, string inviteCode, DateTime utcNow)
    {
        var server = new Server
        {
            Id = Guid.NewGuid(),
            OwnerId = ownerId,
            Name = name,
            IconUrl = iconUrl,
            InviteCode = inviteCode,
            CreatedAt = utcNow
        };

        server.Members.Add(new ServerMember
        {
            Id = Guid.NewGuid(),
            UserId = ownerId,
            JoinedAt = utcNow,
            CreatedAt = utcNow
        });

        server.Roles.Add(new ServerRole
        {
            Id = Guid.NewGuid(),
            Name = "@everyone",
            IsDefault = true,
            Position = 0,
            Permissions = DefaultEveryonePermissions,
            CreatedAt = utcNow
        });

        server.Channels.Add(new Channel
        {
            Id = Guid.NewGuid(),
            Name = "general",
            Type = ChannelType.Text,
            Position = 0,
            CreatedAt = utcNow
        });

        return server;
    }
}
