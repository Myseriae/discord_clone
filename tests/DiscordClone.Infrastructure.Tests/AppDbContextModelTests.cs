using DiscordClone.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Infrastructure.Tests;

public class AppDbContextModelTests
{
    private static AppDbContext CreateContext()
    {
        // A connection string is needed to build the model, but no connection is opened
        // when we only inspect context.Model — this validates all Fluent configurations.
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql("Host=localhost;Database=model-validation;Username=x;Password=y")
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void Model_BuildsAndContainsCoreEntities()
    {
        using var context = CreateContext();

        var tableNames = context.Model.GetEntityTypes()
            .Select(e => e.GetTableName())
            .ToList();

        tableNames.Should().Contain(["Servers", "Channels", "Messages", "ServerMemberRoles", "RefreshTokens"]);
        tableNames.Should().Contain("AspNetUsers"); // Identity wired via IdentityDbContext
    }

    [Fact]
    public void ChannelType_IsStoredAsString()
    {
        using var context = CreateContext();

        var typeColumn = context.Model
            .FindEntityType(typeof(DiscordClone.Domain.Entities.Channel))!
            .FindProperty(nameof(DiscordClone.Domain.Entities.Channel.Type))!;

        typeColumn.GetProviderClrType().Should().Be(typeof(string));
    }
}
