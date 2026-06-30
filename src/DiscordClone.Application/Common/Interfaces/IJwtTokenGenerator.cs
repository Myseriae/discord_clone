using DiscordClone.Application.Common.Models;

namespace DiscordClone.Application.Common.Interfaces;

/// <summary>Issues signed JWT access tokens for an authenticated user.</summary>
public interface IJwtTokenGenerator
{
    AccessToken GenerateAccessToken(AuthUser user);
}
