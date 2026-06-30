using DiscordClone.Application.Auth.Common;
using DiscordClone.Application.Common.Interfaces;
using ErrorOr;
using MediatR;

namespace DiscordClone.Application.Auth.Commands.Refresh;

public class RefreshTokenCommandHandler(
    IIdentityService identityService,
    IJwtTokenGenerator tokenGenerator,
    IRefreshTokenService refreshTokenService)
    : IRequestHandler<RefreshTokenCommand, ErrorOr<AuthResult>>
{
    public async Task<ErrorOr<AuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var userIdResult = await refreshTokenService.ValidateAndRevokeAsync(request.RefreshToken, cancellationToken);
        if (userIdResult.IsError)
        {
            return userIdResult.Errors;
        }

        var userResult = await identityService.GetByIdAsync(userIdResult.Value, cancellationToken);
        if (userResult.IsError)
        {
            return userResult.Errors;
        }

        var user = userResult.Value;
        var accessToken = tokenGenerator.GenerateAccessToken(user);
        var refreshToken = await refreshTokenService.IssueAsync(user.Id, cancellationToken);

        return new AuthResult(
            user.Id, user.Email, user.DisplayName,
            accessToken.Value, accessToken.ExpiresAtUtc, refreshToken.Token);
    }
}
