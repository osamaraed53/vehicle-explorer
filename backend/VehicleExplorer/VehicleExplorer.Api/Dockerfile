# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    ASPNETCORE_HTTP_PORTS=8080
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["VehicleExplorer.Api/VehicleExplorer.Api.csproj", "VehicleExplorer.Api/"]
RUN dotnet restore "VehicleExplorer.Api/VehicleExplorer.Api.csproj"
COPY . .
WORKDIR "/src/VehicleExplorer.Api"
RUN dotnet publish "VehicleExplorer.Api.csproj" \
    -c "$BUILD_CONFIGURATION" \
    -o /app/publish \
    /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "VehicleExplorer.Api.dll"]
