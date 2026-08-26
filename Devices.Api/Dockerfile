FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Devices.Api/Devices.Api.csproj", "Devices.Api/"]
COPY ["Devices.Application/Devices.Application.csproj", "Devices.Application/"]
COPY ["Devices.Domain/Devices.Domain.csproj", "Devices.Domain/"]
COPY ["Devices.Infrastructure/Devices.Infrastructure.csproj", "Devices.Infrastructure/"]

RUN dotnet restore "Devices.Api/Devices.Api.csproj"
COPY . .
WORKDIR "/src/Devices.Api"
RUN dotnet build "./Devices.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Devices.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Devices.Api.dll"]