using System.Security.Cryptography;
using DiscordClone.Application.Auth.Common;
using DiscordClone.Application.Common.Interfaces;
using DiscordClone.Application.Common.Models;
using DiscordClone.Infrastructure.Identity;
using DiscordClone.Infrastructure.Persistence;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DiscordClone.Infrastructure.Authentication;

public class RefreshTokenService(
    AppDbContext context,
    IDateTimeProvider dateTimeProvider,
    IOptions<JwtSettings> settings) : IRefreshTokenService
{
    private readonly JwtSettings _settings = settings.Value;

    public async Task<RefreshTokenResult> IssueAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var token = GenerateToken();
        var expiresAt = dateTimeProvider.UtcNow.AddDays(_settings.RefreshTokenDays);

        context.RefreshTokens.Add(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = token,
            ExpiresAt = expiresAt,
            IsRevoked = false,
            CreatedAt = dateTimeProvider.UtcNow
        });
        await context.SaveChangesAsync(cancellationToken);

        return new RefreshTokenResult(token, expiresAt);
    }

    public async Task<ErrorOr<Guid>> ValidateAndRevokeAsync(string token, CancellationToken cancellationToken = default)
    {
        var stored = await context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == token, cancellationToken);

        if (stored is null || stored.IsRevoked || stored.ExpiresAt <= dateTimeProvider.UtcNow)
        {
            return AuthErrors.InvalidRefreshToken;
        }

        stored.IsRevoked = true;
        await context.SaveChangesAsync(cancellationToken);

        return stored.UserId;
    }

    private static string GenerateToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
}
