using FluentValidation;

namespace DiscordClone.Application.Servers.Commands.UpdateServer;

public class UpdateServerCommandValidator : AbstractValidator<UpdateServerCommand>
{
    public UpdateServerCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .When(x => x.Name is not null);

        RuleFor(x => x.IconUrl)
            .MaximumLength(2048)
            .When(x => x.IconUrl is not null);

        RuleFor(x => x)
            .Must(x => x.Name is not null || x.IconUrl is not null)
            .WithMessage("At least one field (Name or IconUrl) must be provided.");
    }
}
