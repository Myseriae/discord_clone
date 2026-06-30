using DiscordClone.Application.Auth.Common;
using DiscordClone.Application.Common.Interfaces;
using ErrorOr;
using MediatR;

namespace DiscordClone.Application.Auth.Commands.Login;

public class LoginCommandHandler(
    IIdentityService identityService,
    IJwtTokenGenerator tokenGenerator,
    IRefreshTokenService refreshTokenService)
    : IRequestHandler<LoginCommand, ErrorOr<AuthResult>>
{
    public async Task<ErrorOr<AuthResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var userResult = await identityService.AuthenticateAsync(
            request.Email, request.Password, cancellationToken);

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
