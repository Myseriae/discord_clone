using AutoMapper;
using DiscordClone.Domain.Entities;

namespace DiscordClone.Application.Servers.Common;

/// <summary>AutoMapper profile for the Servers feature (entities → response DTOs).</summary>
public class ServerMappingProfile : Profile
{
    public ServerMappingProfile()
    {
        CreateMap<Server, ServerResponse>();
        CreateMap<Server, ServerDetailResponse>();
        // Channel → ChannelResponse lives in ChannelMappingProfile (Channels feature).
    }
}
