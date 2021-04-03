FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore GhostNetwork.Profiles.Grpc/GhostNetwork.Profiles.Grpc.csproj
WORKDIR /src/GhostNetwork.Profiles.Grpc
RUN dotnet build GhostNetwork.Profiles.Grpc.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish GhostNetwork.Profiles.Grpc.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GhostNetwork.Profiles.Grpc.dll"]
