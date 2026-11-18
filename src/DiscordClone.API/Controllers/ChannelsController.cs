using DiscordClone.API.Contracts.Channels;
using DiscordClone.Application.Channels.Commands.CreateChannel;
using DiscordClone.Application.Channels.Commands.DeleteChannel;
using DiscordClone.Application.Channels.Commands.UpdateChannel;
using DiscordClone.Application.Channels.Queries.GetChannelById;
using DiscordClone.Application.Channels.Queries.GetServerChannels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiscordClone.API.Controllers;

// Channels are created/listed under their server, but fetched/edited/deleted by their
// (globally unique) id — hence absolute route templates per action rather than one prefix.
[Authorize]
public class ChannelsController(ISender mediator) : ApiController
{
    [HttpPost("api/servers/{serverId:guid}/channels")]
    public async Task<IActionResult> Create(Guid serverId, CreateChannelRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateChannelCommand(serverId, request.Name, request.Type);
        var result = await mediator.Send(command, cancellationToken);
        return result.Match<IActionResult>(
            channel => CreatedAtAction(nameof(GetById), new { id = channel.Id }, channel),
            Problem);
    }

    [HttpGet("api/servers/{serverId:guid}/channels")]
    public async Task<IActionResult> GetForServer(Guid serverId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetServerChannelsQuery(serverId), cancellationToken);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpGet("api/channels/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetChannelByIdQuery(id), cancellationToken);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpPatch("api/channels/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateChannelRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateChannelCommand(id, request.Name, request.Position);
        var result = await mediator.Send(command, cancellationToken);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpDelete("api/channels/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteChannelCommand(id), cancellationToken);
        return result.Match<IActionResult>(_ => NoContent(), Problem);
    }
}
