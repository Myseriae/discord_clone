using Microsoft.AspNetCore.Identity;

namespace DiscordClone.Infrastructure.Identity;

/// <summary>
/// The ASP.NET Identity user. Lives in Infrastructure (not Domain) because Identity is an
/// infrastructure concern — Domain entities reference users by <see cref="Guid"/> only.
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    public string DisplayName { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
