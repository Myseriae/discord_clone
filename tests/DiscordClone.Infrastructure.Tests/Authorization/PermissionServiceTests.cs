using DiscordClone.Domain.Constants;
using DiscordClone.Domain.Entities;
using DiscordClone.Infrastructure.Authorization;
using DiscordClone.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Infrastructure.Tests.Authorization;

public class PermissionServiceTests
{
    private static readonly DateTime Now = new(2026, 06, 29, 12, 0, 0, DateTimeKind.Utc);

    // A fresh isolated in-memory database per context. Note: InMemory does not enforce relational
    // constraints — adequate here because we're exercising permission *resolution* logic only.
    private static AppDbContext CreateContext() =>
        new(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    [Fact]
    public async Task Owner_HasEveryPermission()
    {
        await using var context = CreateContext();
        var ownerId = Guid.NewGuid();
        var server = Server.Create(ownerId, "S", null, "code", Now);
        context.Servers.Add(server);
        await context.SaveChangesAsync();

        var sut = new PermissionService(context);

        (await sut.HasPermissionAsync(ownerId, server.Id, Permissions.ManageServer)).Should().BeTrue();
        (await sut.HasPermissionAsync(ownerId, server.Id, Permissions.BanMembers)).Should().BeTrue();
    }

    [Fact]
    public async Task Member_GetsEveryoneBaseline_ButNotElevatedPermissions()
    {
        await using var context = CreateContext();
        var server = Server.Create(Guid.NewGuid(), "S", null, "code", Now);
        var memberUserId = Guid.NewGuid();
        server.Members.Add(new ServerMember
        {
            Id = Guid.NewGuid(), UserId = memberUserId, JoinedAt = Now, CreatedAt = Now
        });
        context.Servers.Add(server);
        await context.SaveChangesAsync();

        var sut = new PermissionService(context);

        // @everyone baseline grants ViewChannels...
        (await sut.HasPermissionAsync(memberUserId, server.Id, Permissions.ViewChannels)).Should().BeTrue();
        // ...but not ManageServer.
        (await sut.HasPermissionAsync(memberUserId, server.Id, Permissions.ManageServer)).Should().BeFalse();
    }

    [Fact]
    public async Task Member_WithRoleGrantingPermission_HasIt()
    {
        await using var context = CreateContext();
        var server = Server.Create(Guid.NewGuid(), "S", null, "code", Now);
        var memberUserId = Guid.NewGuid();
        var role = new ServerRole
        {
            Id = Guid.NewGuid(), ServerId = server.Id, Name = "Mods",
            Permissions = Permissions.ManageServer, CreatedAt = Now
        };
        var member = new ServerMember
        {
            Id = Guid.NewGuid(), UserId = memberUserId, JoinedAt = Now, CreatedAt = Now
        };
        member.MemberRoles.Add(new ServerMemberRole
        {
            Id = Guid.NewGuid(), ServerRoleId = role.Id, CreatedAt = Now
        });
        server.Roles.Add(role);
        server.Members.Add(member);
        context.Servers.Add(server);
        await context.SaveChangesAsync();

        var sut = new PermissionService(context);

        (await sut.HasPermissionAsync(memberUserId, server.Id, Permissions.ManageServer)).Should().BeTrue();
    }

    [Fact]
    public async Task NonMember_HasNoPermissions()
    {
        await using var context = CreateContext();
        var server = Server.Create(Guid.NewGuid(), "S", null, "code", Now);
        context.Servers.Add(server);
        await context.SaveChangesAsync();

        var sut = new PermissionService(context);

        (await sut.HasPermissionAsync(Guid.NewGuid(), server.Id, Permissions.ViewChannels)).Should().BeFalse();
    }
}
