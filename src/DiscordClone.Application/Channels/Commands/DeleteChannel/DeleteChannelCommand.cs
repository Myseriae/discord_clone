using ErrorOr;
using MediatR;

namespace DiscordClone.Application.Channels.Commands.DeleteChannel;

public record DeleteChannelCommand(Guid ChannelId) : IRequest<ErrorOr<Deleted>>;
