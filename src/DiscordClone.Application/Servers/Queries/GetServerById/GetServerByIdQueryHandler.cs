using AutoMapper;
using AutoMapper.QueryableExtensions;
using DiscordClone.Application.Common.Interfaces;
using DiscordClone.Application.Servers.Common;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Application.Servers.Queries.GetServerById;

public class GetServerByIdQueryHandler(
    ICurrentUserService currentUser,
    IApplicationDbContext context,
    IMapper mapper) : IRequestHandler<GetServerByIdQuery, ErrorOr<ServerDetailResponse>>
{
    public async Task<ErrorOr<ServerDetailResponse>> Handle(GetServerByIdQuery request, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId)
        {
            return Error.Unauthorized();
        }

        // Membership-filtered: a non-member sees the same NotFound as a missing server (no existence leak).
        var server = await context.Servers
            .Where(s => s.Id == request.ServerId && s.Members.Any(m => m.UserId == userId))
            .ProjectTo<ServerDetailResponse>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (server is null)
        {
            return ServerErrors.NotFound;
        }

        return server;
    }
}
