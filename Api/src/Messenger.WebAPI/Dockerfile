FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Messenger.WebAPI/Messenger.WebAPI.csproj", "Messenger.WebAPI/"]
COPY ["src/Messenger.Application/Messenger.Application.csproj", "Messenger.Application/"]
COPY ["src/Messenger.Domain/Messenger.Domain.csproj", "Messenger.Domain/"]
COPY ["src/Messenger.Infrastructure/Messenger.Infrastructure.csproj", "Messenger.Infrastructure/"]
RUN dotnet restore "./Messenger.WebAPI/Messenger.WebAPI.csproj"
COPY . .
WORKDIR "/src/Messenger.WebAPI"
RUN dotnet build "./Messenger.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Messenger.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Messenger.WebAPI.dll"]