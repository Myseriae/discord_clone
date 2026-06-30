using DiscordClone.Application.Common.Interfaces;
using DiscordClone.Application.Servers.Common;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Application.Servers.Commands.DeleteServer;

public class DeleteServerCommandHandler(
    ICurrentUserService currentUser,
    IApplicationDbContext context) : IRequestHandler<DeleteServerCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteServerCommand request, CancellationToken cancellationToken)
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

        // Deleting a server is owner-only — not a role-grantable permission.
        if (server.OwnerId != userId)
        {
            return ServerErrors.Forbidden;
        }

        context.Servers.Remove(server);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}
