using AutoMapper;
using AutoMapper.QueryableExtensions;
using DiscordClone.Application.Channels.Common;
using DiscordClone.Application.Common.Interfaces;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Application.Channels.Queries.GetChannelById;

public class GetChannelByIdQueryHandler(
    ICurrentUserService currentUser,
    IApplicationDbContext context,
    IMapper mapper) : IRequestHandler<GetChannelByIdQuery, ErrorOr<ChannelResponse>>
{
    public async Task<ErrorOr<ChannelResponse>> Handle(GetChannelByIdQuery request, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId)
        {
            return Error.Unauthorized();
        }

        // Membership-filtered via the channel's server: non-members get NotFound.
        var channel = await context.Channels
            .Where(c => c.Id == request.ChannelId && c.Server.Members.Any(m => m.UserId == userId))
            .ProjectTo<ChannelResponse>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (channel is null)
        {
            return ChannelErrors.NotFound;
        }

        return channel;
    }
}
