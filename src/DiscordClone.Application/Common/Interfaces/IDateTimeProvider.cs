namespace DiscordClone.Application.Common.Interfaces;

/// <summary>Abstracts the system clock so time-dependent logic stays testable.</summary>
public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
