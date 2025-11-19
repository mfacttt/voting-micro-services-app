FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . ./

RUN dotnet restore "src/Api/VoteCastingService.Api/VoteCastingService.Api.csproj"
RUN dotnet publish "src/Api/VoteCastingService.Api/VoteCastingService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "VoteCastingService.Api.dll"]
