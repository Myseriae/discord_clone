using DiscordClone.Application.Servers.Common;
using ErrorOr;
using MediatR;

namespace DiscordClone.Application.Servers.Commands.CreateServer;

public record CreateServerCommand(string Name, string? IconUrl) : IRequest<ErrorOr<ServerResponse>>;
