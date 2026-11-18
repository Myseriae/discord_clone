using FluentValidation;

namespace DiscordClone.Application.Channels.Commands.UpdateChannel;

public class UpdateChannelCommandValidator : AbstractValidator<UpdateChannelCommand>
{
    public UpdateChannelCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .When(x => x.Name is not null);

        RuleFor(x => x.Position)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Position is not null);

        RuleFor(x => x)
            .Must(x => x.Name is not null || x.Position is not null)
            .WithMessage("At least one field (Name or Position) must be provided.");
    }
}
