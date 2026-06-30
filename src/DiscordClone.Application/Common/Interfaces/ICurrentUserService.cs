namespace DiscordClone.Application.Common.Interfaces;

/// <summary>
/// Exposes the authenticated caller's id to the Application layer, sourced from the JWT.
/// Implemented in the API layer over IHttpContextAccessor so handlers never touch HttpContext.
/// </summary>
public interface ICurrentUserService
{
    Guid? UserId { get; }
}
