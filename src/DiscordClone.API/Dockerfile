# Build context is the repository root (see docker-compose.yml).
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution-wide build files and project manifests first for restore-layer caching.
COPY global.json Directory.Build.props DiscordClone.sln ./
COPY src/DiscordClone.Domain/DiscordClone.Domain.csproj src/DiscordClone.Domain/
COPY src/DiscordClone.Application/DiscordClone.Application.csproj src/DiscordClone.Application/
COPY src/DiscordClone.Infrastructure/DiscordClone.Infrastructure.csproj src/DiscordClone.Infrastructure/
COPY src/DiscordClone.API/DiscordClone.API.csproj src/DiscordClone.API/

RUN dotnet restore src/DiscordClone.API/DiscordClone.API.csproj

COPY src/ src/
RUN dotnet publish src/DiscordClone.API/DiscordClone.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DiscordClone.API.dll"]
