using DiscordClone.Application.Channels.Common;
using ErrorOr;
using MediatR;

namespace DiscordClone.Application.Channels.Commands.CreateChannel;

public record CreateChannelCommand(Guid ServerId, string Name, string Type) : IRequest<ErrorOr<ChannelResponse>>;
