using DiscordClone.Domain.Common;
using DiscordClone.Domain.Constants;
using FluentAssertions;

namespace DiscordClone.Domain.Tests;

public class PermissionResolverTests
{
    [Fact]
    public void Combine_OrsAllRolePermissions()
    {
        var combined = PermissionResolver.Combine(new[]
        {
            Permissions.ViewChannels,
            Permissions.SendMessages | Permissions.KickMembers
        });

        combined.Should().Be(Permissions.ViewChannels | Permissions.SendMessages | Permissions.KickMembers);
    }

    [Fact]
    public void Combine_EmptyReturnsNone()
    {
        PermissionResolver.Combine(Array.Empty<long>()).Should().Be(Permissions.None);
    }

    [Fact]
    public void Has_ReturnsTrueOnlyWhenAllRequiredBitsPresent()
    {
        var permissions = Permissions.ViewChannels | Permissions.ManageServer;

        PermissionResolver.Has(permissions, Permissions.ManageServer).Should().BeTrue();
        PermissionResolver.Has(permissions, Permissions.BanMembers).Should().BeFalse();
        // Requires ALL bits: has ViewChannels but not BanMembers → false.
        PermissionResolver.Has(permissions, Permissions.ViewChannels | Permissions.BanMembers).Should().BeFalse();
    }
}
