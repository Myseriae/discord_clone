using DiscordClone.Application.Auth.Common;
using DiscordClone.Application.Common.Interfaces;
using ErrorOr;
using MediatR;

namespace DiscordClone.Application.Auth.Commands.Register;

public class RegisterCommandHandler(
    IIdentityService identityService,
    IJwtTokenGenerator tokenGenerator,
    IRefreshTokenService refreshTokenService)
    : IRequestHandler<RegisterCommand, ErrorOr<AuthResult>>
{
    public async Task<ErrorOr<AuthResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var userResult = await identityService.RegisterAsync(
            request.Email, request.DisplayName, request.Password, cancellationToken);

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
