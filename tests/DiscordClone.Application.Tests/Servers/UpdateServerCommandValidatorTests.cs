using DiscordClone.Application.Servers.Commands.UpdateServer;
using FluentAssertions;

namespace DiscordClone.Application.Tests.Servers;

public class UpdateServerCommandValidatorTests
{
    private readonly UpdateServerCommandValidator _validator = new();

    [Fact]
    public void Validate_WithNameProvided_Passes()
    {
        var result = _validator.Validate(new UpdateServerCommand(Guid.NewGuid(), "New Name", null));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithNoFieldsProvided_Fails()
    {
        var result = _validator.Validate(new UpdateServerCommand(Guid.NewGuid(), null, null));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithEmptyName_Fails()
    {
        var result = _validator.Validate(new UpdateServerCommand(Guid.NewGuid(), "", null));

        result.IsValid.Should().BeFalse();
    }
}
