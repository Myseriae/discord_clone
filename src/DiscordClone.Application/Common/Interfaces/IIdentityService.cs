using DiscordClone.Application.Common.Models;
using ErrorOr;

namespace DiscordClone.Application.Common.Interfaces;

/// <summary>
/// Abstracts ASP.NET Identity user management so the Application layer can register and
/// authenticate users without depending on Infrastructure.
/// </summary>
public interface IIdentityService
{
    Task<ErrorOr<AuthUser>> RegisterAsync(
        string email, string displayName, string password, CancellationToken cancellationToken = default);

    Task<ErrorOr<AuthUser>> AuthenticateAsync(
        string email, string password, CancellationToken cancellationToken = default);

    Task<ErrorOr<AuthUser>> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
