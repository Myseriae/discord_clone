using DiscordClone.API.Contracts.Auth;
using DiscordClone.Application.Auth.Commands.Login;
using DiscordClone.Application.Auth.Commands.Refresh;
using DiscordClone.Application.Auth.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DiscordClone.API.Controllers;

[Route("api/auth")]
public class AuthController(ISender mediator) : ApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterCommand(request.Email, request.DisplayName, request.Password);
        var result = await mediator.Send(command, cancellationToken);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var result = await mediator.Send(command, cancellationToken);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand(request.RefreshToken);
        var result = await mediator.Send(command, cancellationToken);
        return result.Match<IActionResult>(Ok, Problem);
    }
}
