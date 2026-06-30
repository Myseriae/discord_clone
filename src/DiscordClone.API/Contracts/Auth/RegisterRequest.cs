namespace DiscordClone.API.Contracts.Auth;

public record RegisterRequest(string Email, string DisplayName, string Password);
