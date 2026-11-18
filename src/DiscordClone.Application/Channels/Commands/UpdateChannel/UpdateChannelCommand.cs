using DiscordClone.Application.Channels.Common;
using ErrorOr;
using MediatR;

namespace DiscordClone.Application.Channels.Commands.UpdateChannel;

public record UpdateChannelCommand(Guid ChannelId, string? Name, int? Position) : IRequest<ErrorOr<ChannelResponse>>;
