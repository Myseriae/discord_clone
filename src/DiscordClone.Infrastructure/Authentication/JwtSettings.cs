namespace DiscordClone.Infrastructure.Authentication;

/// <summary>Strongly-typed binding of the "Jwt" configuration section.</summary>
public class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public string Secret { get; init; } = null!;
    public int AccessTokenMinutes { get; init; }
    public int RefreshTokenDays { get; init; }
}
