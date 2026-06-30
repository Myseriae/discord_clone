using DiscordClone.API.Contracts.Servers;
using DiscordClone.Application.Servers.Commands.CreateServer;
using DiscordClone.Application.Servers.Commands.DeleteServer;
using DiscordClone.Application.Servers.Commands.UpdateServer;
using DiscordClone.Application.Servers.Queries.GetMyServers;
using DiscordClone.Application.Servers.Queries.GetServerById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiscordClone.API.Controllers;

[Authorize]
[Route("api/servers")]
public class ServersController(ISender mediator) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateServerRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateServerCommand(request.Name, request.IconUrl);
        var result = await mediator.Send(command, cancellationToken);
        return result.Match<IActionResult>(
            server => CreatedAtAction(nameof(GetById), new { id = server.Id }, server),
            Problem);
    }

    [HttpGet]
    public async Task<IActionResult> GetMine(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetMyServersQuery(), cancellationToken);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetServerByIdQuery(id), cancellationToken);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateServerRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateServerCommand(id, request.Name, request.IconUrl);
        var result = await mediator.Send(command, cancellationToken);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteServerCommand(id), cancellationToken);
        return result.Match<IActionResult>(_ => NoContent(), Problem);
    }
}
