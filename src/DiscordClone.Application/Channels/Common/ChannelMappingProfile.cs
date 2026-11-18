using AutoMapper;
using DiscordClone.Domain.Entities;

namespace DiscordClone.Application.Channels.Common;

/// <summary>AutoMapper profile for the Channels feature. ChannelType (enum) maps to its string name.</summary>
public class ChannelMappingProfile : Profile
{
    public ChannelMappingProfile()
    {
        CreateMap<Channel, ChannelResponse>();
    }
}
