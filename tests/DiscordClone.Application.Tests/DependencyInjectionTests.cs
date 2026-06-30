using AutoMapper;
using DiscordClone.Application;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordClone.Application.Tests;

public class DependencyInjectionTests
{
    [Fact]
    public void AddApplication_RegistersMediatRAndAutoMapper()
    {
        var services = new ServiceCollection().AddApplication();

        services.Should().Contain(d => d.ServiceType == typeof(IMediator));
        services.Should().Contain(d => d.ServiceType == typeof(IMapper));
    }
}
