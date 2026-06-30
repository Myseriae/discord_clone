using AutoMapper;
using AutoMapper.QueryableExtensions;
using DiscordClone.Application.Common.Interfaces;
using DiscordClone.Application.Servers.Common;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Application.Servers.Queries.GetMyServers;

public class GetMyServersQueryHandler(
    ICurrentUserService currentUser,
    IApplicationDbContext context,
    IMapper mapper) : IRequestHandler<GetMyServersQuery, ErrorOr<IReadOnlyList<ServerResponse>>>
{
    public async Task<ErrorOr<IReadOnlyList<ServerResponse>>> Handle(GetMyServersQuery request, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId)
        {
            return Error.Unauthorized();
        }

        var servers = await context.ServerMembers
            .Where(m => m.UserId == userId)
            .Select(m => m.Server)
            .ProjectTo<ServerResponse>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return servers;
    }
}
