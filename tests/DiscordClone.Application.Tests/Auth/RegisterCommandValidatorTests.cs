using DiscordClone.Application.Auth.Commands.Register;
using FluentAssertions;

namespace DiscordClone.Application.Tests.Auth;

public class RegisterCommandValidatorTests
{
    private readonly RegisterCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_Passes()
    {
        var command = new RegisterCommand("user@example.com", "Alice", "supersecret");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("not-an-email", "Alice", "supersecret")]
    [InlineData("user@example.com", "", "supersecret")]
    [InlineData("user@example.com", "Alice", "short")]
    public void Validate_WithInvalidCommand_Fails(string email, string displayName, string password)
    {
        var command = new RegisterCommand(email, displayName, password);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }
}
