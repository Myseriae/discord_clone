using FluentValidation;

namespace DiscordClone.Application.Servers.Commands.CreateServer;

public class CreateServerCommandValidator : AbstractValidator<CreateServerCommand>
{
    public CreateServerCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.IconUrl)
            .MaximumLength(2048)
            .When(x => x.IconUrl is not null);
    }
}
