namespace DiscordClone.Application.Common.Interfaces;

/// <summary>
/// Resolves whether a user holds a permission within a server, accounting for owner override,
/// the member's assigned roles, and the server's "@everyone" baseline.
/// </summary>
public interface IPermissionService
{
    Task<bool> HasPermissionAsync(Guid userId, Guid serverId, long permission, CancellationToken cancellationToken = default);
}
