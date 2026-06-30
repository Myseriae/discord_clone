using DiscordClone.Application.Common.Interfaces;
using DiscordClone.Domain.Common;
using DiscordClone.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Infrastructure.Authorization;

/// <summary>
/// Resolves a user's effective permissions within a server:
/// owner ⇒ all permissions; otherwise the OR of the member's assigned role permissions
/// combined with the server's "@everyone" baseline role.
/// </summary>
public class PermissionService(AppDbContext context) : IPermissionService
{
    public async Task<bool> HasPermissionAsync(
        Guid userId, Guid serverId, long permission, CancellationToken cancellationToken = default)
    {
        var server = await context.Servers
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == serverId, cancellationToken);

        if (server is null)
        {
            return false;
        }

        // The owner implicitly holds every permission.
        if (server.OwnerId == userId)
        {
            return true;
        }

        var member = await context.ServerMembers
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ServerId == serverId && m.UserId == userId, cancellationToken);

        if (member is null)
        {
            return false;
        }

        // The member's explicitly-assigned role permissions...
        var assignedPermissions = await context.ServerMemberRoles
            .AsNoTracking()
            .Where(mr => mr.ServerMemberId == member.Id)
            .Select(mr => mr.ServerRole.Permissions)
            .ToListAsync(cancellationToken);

        // ...plus the server's "@everyone" baseline, which applies to all members.
        var everyonePermissions = await context.ServerRoles
            .AsNoTracking()
            .Where(r => r.ServerId == serverId && r.IsDefault)
            .Select(r => r.Permissions)
            .ToListAsync(cancellationToken);

        var combined = PermissionResolver.Combine(assignedPermissions.Concat(everyonePermissions));
        return PermissionResolver.Has(combined, permission);
    }
}
