# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated9.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 7071

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Messenger.AzureFunctions/Messenger.AzureFunctions.csproj", "src/Messenger.AzureFunctions/"]
COPY ["src/Messenger.Application/Messenger.Application.csproj", "src/Messenger.Application/"]
COPY ["src/Messenger.Domain/Messenger.Domain.csproj", "src/Messenger.Domain/"]
COPY ["src/Messenger.Infrastructure/Messenger.Infrastructure.csproj", "src/Messenger.Infrastructure/"]
RUN dotnet restore "./src/Messenger.AzureFunctions/Messenger.AzureFunctions.csproj"
COPY . .
WORKDIR "/src/src/Messenger.AzureFunctions"
RUN dotnet build "./Messenger.AzureFunctions.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Messenger.AzureFunctions.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true