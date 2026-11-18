using DiscordClone.Domain.Enums;
using FluentValidation;

namespace DiscordClone.Application.Channels.Commands.CreateChannel;

public class CreateChannelCommandValidator : AbstractValidator<CreateChannelCommand>
{
    public CreateChannelCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Type)
            .Must(BeACreatableChannelType)
            .WithMessage("Channel type must be one of: Text, Voice, Announcement.");
    }

    // Valid ChannelType, excluding Category (categories/nesting are a later slice).
    private static bool BeACreatableChannelType(string type) =>
        Enum.TryParse<ChannelType>(type, ignoreCase: true, out var parsed) && parsed != ChannelType.Category;
}
