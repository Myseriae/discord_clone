using ErrorOr;
using MediatR;

namespace DiscordClone.Application.Servers.Commands.DeleteServer;

public record DeleteServerCommand(Guid ServerId) : IRequest<ErrorOr<Deleted>>;
