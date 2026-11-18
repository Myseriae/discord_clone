using DiscordClone.Application.Channels.Commands.CreateChannel;
using FluentAssertions;

namespace DiscordClone.Application.Tests.Channels;

public class CreateChannelCommandValidatorTests
{
    private readonly CreateChannelCommandValidator _validator = new();

    [Theory]
    [InlineData("Text")]
    [InlineData("Voice")]
    [InlineData("Announcement")]
    [InlineData("text")] // case-insensitive
    public void Validate_WithCreatableType_Passes(string type)
    {
        var result = _validator.Validate(new CreateChannelCommand(Guid.NewGuid(), "general", type));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithCategoryType_Fails()
    {
        var result = _validator.Validate(new CreateChannelCommand(Guid.NewGuid(), "general", "Category"));

        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("Nonsense")]
    [InlineData("")]
    public void Validate_WithInvalidType_Fails(string type)
    {
        var result = _validator.Validate(new CreateChannelCommand(Guid.NewGuid(), "general", type));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithEmptyName_Fails()
    {
        var result = _validator.Validate(new CreateChannelCommand(Guid.NewGuid(), "", "Text"));

        result.IsValid.Should().BeFalse();
    }
}
