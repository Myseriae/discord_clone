using DiscordClone.Domain.Constants;
using DiscordClone.Domain.Entities;
using DiscordClone.Domain.Enums;
using FluentAssertions;

namespace DiscordClone.Domain.Tests;

public class ServerFactoryTests
{
    [Fact]
    public void Create_SeedsOwnerMemberDefaultRoleAndGeneralChannel()
    {
        var ownerId = Guid.NewGuid();
        var now = new DateTime(2026, 06, 29, 12, 0, 0, DateTimeKind.Utc);

        var server = Server.Create(ownerId, "Gaming", iconUrl: null, inviteCode: "invite01", utcNow: now);

        server.OwnerId.Should().Be(ownerId);
        server.Name.Should().Be("Gaming");
        server.InviteCode.Should().Be("invite01");
        server.CreatedAt.Should().Be(now);

        server.Members.Should().ContainSingle()
            .Which.UserId.Should().Be(ownerId);

        var role = server.Roles.Should().ContainSingle().Subject;
        role.IsDefault.Should().BeTrue();
        role.Name.Should().Be("@everyone");
        role.Permissions.Should().Be(Permissions.ViewChannels | Permissions.SendMessages);

        var channel = server.Channels.Should().ContainSingle().Subject;
        channel.Name.Should().Be("general");
        channel.Type.Should().Be(ChannelType.Text);
    }
}
