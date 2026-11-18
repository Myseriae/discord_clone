using AutoMapper;
using DiscordClone.Application.Channels.Commands.CreateChannel;
using DiscordClone.Application.Channels.Common;
using DiscordClone.Application.Common.Interfaces;
using DiscordClone.Domain.Entities;
using DiscordClone.Infrastructure.Authorization;
using DiscordClone.Infrastructure.Persistence;
using ErrorOr;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace DiscordClone.Infrastructure.Tests.Channels;

public class CreateChannelCommandHandlerTests
{
    private static readonly DateTime Now = new(2026, 06, 30, 12, 0, 0, DateTimeKind.Utc);

    private static AppDbContext CreateContext() =>
        new(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static IMapper CreateMapper() =>
        new MapperConfiguration(cfg => cfg.AddProfile<ChannelMappingProfile>(), NullLoggerFactory.Instance)
            .CreateMapper();

    private static CreateChannelCommandHandler CreateHandler(AppDbContext context, Guid? currentUserId)
    {
        var currentUser = new Mock<ICurrentUserService>();
        currentUser.SetupGet(c => c.UserId).Returns(currentUserId);

        var clock = new Mock<IDateTimeProvider>();
        clock.SetupGet(c => c.UtcNow).Returns(Now);

        return new CreateChannelCommandHandler(
            currentUser.Object, context, new PermissionService(context), clock.Object, CreateMapper());
    }

    [Fact]
    public async Task Handle_AsOwner_CreatesChannelWithNextPosition()
    {
        await using var context = CreateContext();
        var ownerId = Guid.NewGuid();
        var server = Server.Create(ownerId, "S", null, "code", Now);
        context.Servers.Add(server);
        await context.SaveChangesAsync();

        var handler = CreateHandler(context, ownerId);
        var result = await handler.Handle(
            new CreateChannelCommand(server.Id, "announcements", "Announcement"), CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.Name.Should().Be("announcements");
        result.Value.Type.Should().Be("Announcement");
        result.Value.Position.Should().Be(1); // the seeded "general" channel occupies position 0
    }

    [Fact]
    public async Task Handle_AsNonMember_ReturnsForbidden()
    {
        await using var context = CreateContext();
        var server = Server.Create(Guid.NewGuid(), "S", null, "code", Now);
        context.Servers.Add(server);
        await context.SaveChangesAsync();

        var handler = CreateHandler(context, Guid.NewGuid()); // a random non-member
        var result = await handler.Handle(
            new CreateChannelCommand(server.Id, "x", "Text"), CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Forbidden);
    }
}
