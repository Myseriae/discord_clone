using DiscordClone.Application.Auth.Common;
using ErrorOr;
using MediatR;

namespace DiscordClone.Application.Auth.Commands.Refresh;

public record RefreshTokenCommand(string RefreshToken) : IRequest<ErrorOr<AuthResult>>;
