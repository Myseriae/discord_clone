using DiscordClone.Application.Common.Models;
using ErrorOr;

namespace DiscordClone.Application.Common.Interfaces;

/// <summary>Issues and rotates persisted refresh tokens.</summary>
public interface IRefreshTokenService
{
    Task<RefreshTokenResult> IssueAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates a refresh token and revokes it in the same step (single-use rotation).
    /// Returns the owning user id on success.
    /// </summary>
    Task<ErrorOr<Guid>> ValidateAndRevokeAsync(string token, CancellationToken cancellationToken = default);
}
