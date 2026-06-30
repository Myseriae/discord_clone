using System.IdentityModel.Tokens.Jwt;
using DiscordClone.Application.Common.Interfaces;
using DiscordClone.Application.Common.Models;
using DiscordClone.Infrastructure.Authentication;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;

namespace DiscordClone.Infrastructure.Tests.Authentication;

public class JwtTokenGeneratorTests
{
    private static readonly JwtSettings Settings = new()
    {
        Issuer = "DiscordClone",
        Audience = "DiscordCloneClient",
        Secret = "test-signing-key-that-is-at-least-32-bytes-long",
        AccessTokenMinutes = 15,
        RefreshTokenDays = 7
    };

    [Fact]
    public void GenerateAccessToken_EmbedsUserClaimsAndExpiry()
    {
        var now = new DateTime(2026, 06, 26, 12, 00, 00, DateTimeKind.Utc);
        var clock = new Mock<IDateTimeProvider>();
        clock.SetupGet(c => c.UtcNow).Returns(now);

        var generator = new JwtTokenGenerator(Options.Create(Settings), clock.Object);
        var user = new AuthUser(Guid.NewGuid(), "user@example.com", "Alice");

        var accessToken = generator.GenerateAccessToken(user);

        accessToken.ExpiresAtUtc.Should().Be(now.AddMinutes(Settings.AccessTokenMinutes));

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(accessToken.Value);
        jwt.Issuer.Should().Be(Settings.Issuer);
        jwt.Subject.Should().Be(user.Id.ToString());
        jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == user.Email);
    }
}
