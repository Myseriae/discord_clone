using DiscordClone.Application.Servers.Common;
using ErrorOr;
using MediatR;

namespace DiscordClone.Application.Servers.Queries.GetMyServers;

public record GetMyServersQuery : IRequest<ErrorOr<IReadOnlyList<ServerResponse>>>;
