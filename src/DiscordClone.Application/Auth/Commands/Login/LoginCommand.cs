using DiscordClone.Application.Auth.Common;
using ErrorOr;
using MediatR;

namespace DiscordClone.Application.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<ErrorOr<AuthResult>>;
