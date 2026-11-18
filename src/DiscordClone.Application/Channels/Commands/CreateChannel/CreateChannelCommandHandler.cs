using AutoMapper;
using DiscordClone.Application.Channels.Common;
using DiscordClone.Application.Common.Interfaces;
using DiscordClone.Domain.Constants;
using DiscordClone.Domain.Entities;
using DiscordClone.Domain.Enums;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Application.Channels.Commands.CreateChannel;

public class CreateChannelCommandHandler(
    ICurrentUserService currentUser,
    IApplicationDbContext context,
    IPermissionService permissions,
    IDateTimeProvider dateTimeProvider,
    IMapper mapper) : IRequestHandler<CreateChannelCommand, ErrorOr<ChannelResponse>>
{
    public async Task<ErrorOr<ChannelResponse>> Handle(CreateChannelCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId)
        {
            return Error.Unauthorized();
        }

        if (!await permissions.HasPermissionAsync(userId, request.ServerId, Permissions.ManageChannels, cancellationToken))
        {
            return ChannelErrors.Forbidden;
        }

        // Type is validated by the command validator; parse is safe here.
        var type = Enum.Parse<ChannelType>(request.Type, ignoreCase: true);

        // Auto-assign the next position at the end of the server's channel list.
        var maxPosition = await context.Channels
            .Where(c => c.ServerId == request.ServerId)
            .Select(c => (int?)c.Position)
            .MaxAsync(cancellationToken);

        var channel = new Channel
        {
            Id = Guid.NewGuid(),
            ServerId = request.ServerId,
            Name = request.Name,
            Type = type,
            Position = (maxPosition ?? -1) + 1,
            CreatedAt = dateTimeProvider.UtcNow
        };

        context.Channels.Add(channel);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<ChannelResponse>(channel);
    }
}
