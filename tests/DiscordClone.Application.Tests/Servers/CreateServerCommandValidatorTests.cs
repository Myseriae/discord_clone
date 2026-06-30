using DiscordClone.Application.Servers.Commands.CreateServer;
using FluentAssertions;

namespace DiscordClone.Application.Tests.Servers;

public class CreateServerCommandValidatorTests
{
    private readonly CreateServerCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidName_Passes()
    {
        var result = _validator.Validate(new CreateServerCommand("Gaming", null));

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithEmptyName_Fails(string name)
    {
        var result = _validator.Validate(new CreateServerCommand(name, null));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithTooLongName_Fails()
    {
        var result = _validator.Validate(new CreateServerCommand(new string('a', 101), null));

        result.IsValid.Should().BeFalse();
    }
}
