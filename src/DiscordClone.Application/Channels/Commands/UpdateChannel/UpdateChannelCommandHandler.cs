using AutoMapper;
using DiscordClone.Application.Channels.Common;
using DiscordClone.Application.Common.Interfaces;
using DiscordClone.Domain.Constants;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Application.Channels.Commands.UpdateChannel;

public class UpdateChannelCommandHandler(
    ICurrentUserService currentUser,
    IApplicationDbContext context,
    IPermissionService permissions,
    IMapper mapper) : IRequestHandler<UpdateChannelCommand, ErrorOr<ChannelResponse>>
{
    public async Task<ErrorOr<ChannelResponse>> Handle(UpdateChannelCommand request, CancellationToken cancellationToken)
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

        if (request.Name is not null)
        {
            channel.Name = request.Name;
        }

        if (request.Position is not null)
        {
            channel.Position = request.Position.Value;
        }

        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<ChannelResponse>(channel);
    }
}
