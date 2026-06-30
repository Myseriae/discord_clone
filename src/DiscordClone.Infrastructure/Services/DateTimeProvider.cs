using DiscordClone.Application.Common.Interfaces;

namespace DiscordClone.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
