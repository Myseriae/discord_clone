using DiscordClone.Application.Channels.Commands.UpdateChannel;
using FluentAssertions;

namespace DiscordClone.Application.Tests.Channels;

public class UpdateChannelCommandValidatorTests
{
    private readonly UpdateChannelCommandValidator _validator = new();

    [Fact]
    public void Validate_WithNameProvided_Passes()
    {
        var result = _validator.Validate(new UpdateChannelCommand(Guid.NewGuid(), "renamed", null));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithNoFieldsProvided_Fails()
    {
        var result = _validator.Validate(new UpdateChannelCommand(Guid.NewGuid(), null, null));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithNegativePosition_Fails()
    {
        var result = _validator.Validate(new UpdateChannelCommand(Guid.NewGuid(), null, -1));

        result.IsValid.Should().BeFalse();
    }
}
