using AutoMapper;
using AutoMapper.QueryableExtensions;
using DiscordClone.Application.Channels.Common;
using DiscordClone.Application.Common.Interfaces;
using DiscordClone.Application.Servers.Common;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Application.Channels.Queries.GetServerChannels;

public class GetServerChannelsQueryHandler(
    ICurrentUserService currentUser,
    IApplicationDbContext context,
    IMapper mapper) : IRequestHandler<GetServerChannelsQuery, ErrorOr<IReadOnlyList<ChannelResponse>>>
{
    public async Task<ErrorOr<IReadOnlyList<ChannelResponse>>> Handle(GetServerChannelsQuery request, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId)
        {
            return Error.Unauthorized();
        }

        // Non-members see the same NotFound as a missing server (no existence leak).
        var isMember = await context.ServerMembers
            .AnyAsync(m => m.ServerId == request.ServerId && m.UserId == userId, cancellationToken);
        if (!isMember)
        {
            return ServerErrors.NotFound;
        }

        var channels = await context.Channels
            .Where(c => c.ServerId == request.ServerId)
            .OrderBy(c => c.Position)
            .ProjectTo<ChannelResponse>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return channels;
    }
}
