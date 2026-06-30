using DiscordClone.Application.Servers.Common;
using ErrorOr;
using MediatR;

namespace DiscordClone.Application.Servers.Commands.UpdateServer;

public record UpdateServerCommand(Guid ServerId, string? Name, string? IconUrl) : IRequest<ErrorOr<ServerResponse>>;
