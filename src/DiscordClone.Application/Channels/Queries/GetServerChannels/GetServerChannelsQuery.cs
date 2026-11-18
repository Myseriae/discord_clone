using DiscordClone.Application.Channels.Common;
using ErrorOr;
using MediatR;

namespace DiscordClone.Application.Channels.Queries.GetServerChannels;

public record GetServerChannelsQuery(Guid ServerId) : IRequest<ErrorOr<IReadOnlyList<ChannelResponse>>>;
