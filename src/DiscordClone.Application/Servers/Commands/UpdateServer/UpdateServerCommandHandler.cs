using AutoMapper;
using DiscordClone.Application.Common.Interfaces;
using DiscordClone.Application.Servers.Common;
using DiscordClone.Domain.Constants;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Application.Servers.Commands.UpdateServer;

public class UpdateServerCommandHandler(
    ICurrentUserService currentUser,
    IApplicationDbContext context,
    IPermissionService permissions,
    IMapper mapper) : IRequestHandler<UpdateServerCommand, ErrorOr<ServerResponse>>
{
    public async Task<ErrorOr<ServerResponse>> Handle(UpdateServerCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId)
        {
            return Error.Unauthorized();
        }

        var server = await context.Servers.FirstOrDefaultAsync(s => s.Id == request.ServerId, cancellationToken);
        if (server is null)
        {
            return ServerErrors.NotFound;
        }

        if (!await permissions.HasPermissionAsync(userId, server.Id, Permissions.ManageServer, cancellationToken))
        {
            return ServerErrors.Forbidden;
        }

        if (request.Name is not null)
        {
            server.Name = request.Name;
        }

        if (request.IconUrl is not null)
        {
            server.IconUrl = request.IconUrl;
        }

        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<ServerResponse>(server);
    }
}
