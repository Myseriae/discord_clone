using DiscordClone.Application.Auth.Commands.Register;
using DiscordClone.Application.Common.Interfaces;
using DiscordClone.Application.Common.Models;
using ErrorOr;
using FluentAssertions;
using Moq;

namespace DiscordClone.Application.Tests.Auth;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IIdentityService> _identityService = new();
    private readonly Mock<IJwtTokenGenerator> _tokenGenerator = new();
    private readonly Mock<IRefreshTokenService> _refreshTokenService = new();
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        _handler = new RegisterCommandHandler(
            _identityService.Object, _tokenGenerator.Object, _refreshTokenService.Object);
    }

    [Fact]
    public async Task Handle_WhenRegistrationSucceeds_ReturnsAuthResultWithTokens()
    {
        var command = new RegisterCommand("user@example.com", "Alice", "supersecret");
        var user = new AuthUser(Guid.NewGuid(), command.Email, command.DisplayName);

        _identityService
            .Setup(s => s.RegisterAsync(command.Email, command.DisplayName, command.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _tokenGenerator
            .Setup(g => g.GenerateAccessToken(user))
            .Returns(new AccessToken("access-token", DateTime.UtcNow.AddMinutes(15)));
        _refreshTokenService
            .Setup(s => s.IssueAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new RefreshTokenResult("refresh-token", DateTime.UtcNow.AddDays(7)));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeFalse();
        result.Value.UserId.Should().Be(user.Id);
        result.Value.AccessToken.Should().Be("access-token");
        result.Value.RefreshToken.Should().Be("refresh-token");
    }

    [Fact]
    public async Task Handle_WhenEmailIsDuplicate_ReturnsErrorAndIssuesNoTokens()
    {
        var command = new RegisterCommand("user@example.com", "Alice", "supersecret");

        _identityService
            .Setup(s => s.RegisterAsync(command.Email, command.DisplayName, command.Password, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Error.Conflict("Auth.DuplicateEmail", "exists"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsError.Should().BeTrue();
        result.FirstError.Type.Should().Be(ErrorType.Conflict);
        _refreshTokenService.Verify(s => s.IssueAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
