using DiscordClone.Application.Channels.Common;
using ErrorOr;
using MediatR;

namespace DiscordClone.Application.Channels.Queries.GetChannelById;

public record GetChannelByIdQuery(Guid ChannelId) : IRequest<ErrorOr<ChannelResponse>>;
