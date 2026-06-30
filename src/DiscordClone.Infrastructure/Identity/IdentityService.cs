using DiscordClone.Application.Auth.Common;
using DiscordClone.Application.Common.Interfaces;
using DiscordClone.Application.Common.Models;
using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace DiscordClone.Infrastructure.Identity;

public class IdentityService(UserManager<ApplicationUser> userManager, IDateTimeProvider dateTimeProvider)
    : IIdentityService
{
    public async Task<ErrorOr<AuthUser>> RegisterAsync(
        string email, string displayName, string password, CancellationToken cancellationToken = default)
    {
        if (await userManager.FindByEmailAsync(email) is not null)
        {
            return AuthErrors.DuplicateEmail;
        }

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            Email = email,
            UserName = email,
            DisplayName = displayName,
            CreatedAt = dateTimeProvider.UtcNow
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return result.Errors
                .Select(e => Error.Validation(e.Code, e.Description))
                .ToList();
        }

        return ToAuthUser(user);
    }

    public async Task<ErrorOr<AuthUser>> AuthenticateAsync(
        string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null || !await userManager.CheckPasswordAsync(user, password))
        {
            return AuthErrors.InvalidCredentials;
        }

        return ToAuthUser(user);
    }

    public async Task<ErrorOr<AuthUser>> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return AuthErrors.UserNotFound;
        }

        return ToAuthUser(user);
    }

    private static AuthUser ToAuthUser(ApplicationUser user) =>
        new(user.Id, user.Email!, user.DisplayName);
}
