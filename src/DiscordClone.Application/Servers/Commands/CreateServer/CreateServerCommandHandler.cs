using System.Security.Cryptography;
using AutoMapper;
using DiscordClone.Application.Common.Interfaces;
using DiscordClone.Application.Servers.Common;
using DiscordClone.Domain.Entities;
using ErrorOr;
using MediatR;

namespace DiscordClone.Application.Servers.Commands.CreateServer;

public class CreateServerCommandHandler(
    ICurrentUserService currentUser,
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IMapper mapper) : IRequestHandler<CreateServerCommand, ErrorOr<ServerResponse>>
{
    private const string InviteCodeAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private const int InviteCodeLength = 8;

    public async Task<ErrorOr<ServerResponse>> Handle(CreateServerCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } ownerId)
        {
            return Error.Unauthorized();
        }

        var inviteCode = RandomNumberGenerator.GetString(InviteCodeAlphabet, InviteCodeLength);
        var server = Server.Create(ownerId, request.Name, request.IconUrl, inviteCode, dateTimeProvider.UtcNow);

        context.Servers.Add(server);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<ServerResponse>(server);
    }
}
