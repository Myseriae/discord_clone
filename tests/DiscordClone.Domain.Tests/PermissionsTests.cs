using DiscordClone.Domain.Constants;
using FluentAssertions;

namespace DiscordClone.Domain.Tests;

public class PermissionsTests
{
    [Fact]
    public void Flags_AreDistinctPowersOfTwo()
    {
        long[] flags =
        [
            Permissions.ViewChannels,
            Permissions.SendMessages,
            Permissions.ManageChannels,
            Permissions.ManageRoles,
            Permissions.KickMembers,
            Permissions.MoveMembers,
            Permissions.ManageServer,
            Permissions.BanMembers
        ];

        flags.Should().OnlyHaveUniqueItems();
        flags.Should().AllSatisfy(f => ((f & (f - 1)) == 0).Should().BeTrue("each flag must be a single bit"));
    }

    [Fact]
    public void OrdAndAnd_ComposeAndDetectPermissions()
    {
        var role = Permissions.ViewChannels | Permissions.SendMessages;

        (role & Permissions.ViewChannels).Should().Be(Permissions.ViewChannels);
        (role & Permissions.SendMessages).Should().Be(Permissions.SendMessages);
        (role & Permissions.ManageRoles).Should().Be(Permissions.None);
    }
}
