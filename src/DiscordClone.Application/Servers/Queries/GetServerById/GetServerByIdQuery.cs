using DiscordClone.Application.Servers.Common;
using ErrorOr;
using MediatR;

namespace DiscordClone.Application.Servers.Queries.GetServerById;

public record GetServerByIdQuery(Guid ServerId) : IRequest<ErrorOr<ServerDetailResponse>>;
