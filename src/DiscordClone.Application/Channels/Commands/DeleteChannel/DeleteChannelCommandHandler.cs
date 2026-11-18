using DiscordClone.Application.Channels.Common;
using DiscordClone.Application.Common.Interfaces;
using DiscordClone.Domain.Constants;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Application.Channels.Commands.DeleteChannel;

public class DeleteChannelCommandHandler(
    ICurrentUserService currentUser,
    IApplicationDbContext context,
    IPermissionService permissions) : IRequestHandler<DeleteChannelCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteChannelCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId)
        {
            return Error.Unauthorized();
        }

        var channel = await context.Channels.FirstOrDefaultAsync(c => c.Id == request.ChannelId, cancellationToken);
        if (channel is null)
        {
            return ChannelErrors.NotFound;
        }

        if (!await permissions.HasPermissionAsync(userId, channel.ServerId, Permissions.ManageChannels, cancellationToken))
        {
            return ChannelErrors.Forbidden;
        }

        context.Channels.Remove(channel);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}
