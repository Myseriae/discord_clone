using DiscordClone.Application.Auth.Common;
using ErrorOr;
using MediatR;

namespace DiscordClone.Application.Auth.Commands.Register;

public record RegisterCommand(string Email, string DisplayName, string Password)
    : IRequest<ErrorOr<AuthResult>>;
