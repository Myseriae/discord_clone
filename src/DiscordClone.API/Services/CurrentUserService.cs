using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DiscordClone.Application.Common.Interfaces;

namespace DiscordClone.API.Services;

/// <summary>
/// Reads the authenticated user's id from the JWT "sub" claim on the current HTTP request.
/// Lives in the API layer because identity-from-HttpContext is a web concern.
/// </summary>
public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid? UserId
    {
        get
        {
            var subject = httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return Guid.TryParse(subject, out var userId) ? userId : null;
        }
    }
}
