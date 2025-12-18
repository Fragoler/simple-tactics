FROM node:20-alpine AS frontend-build

WORKDIR /src/frontend

COPY GameFront/package*.json ./
RUN npm ci 

COPY GameFront/ ./
RUN npm run build



FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS backend-build
WORKDIR /src
ARG BUILD_CONFIGURATION=Release

# Restore
COPY GameServer/GameServer.csproj ./GameServer/
COPY Utils/ ./Utils/
RUN dotnet restore GameServer/GameServer.csproj

# Publish
COPY GameServer/ ./GameServer/
RUN dotnet publish GameServer/GameServer.csproj \
    -c ${BUILD_CONFIGURATION} \
    -o /app/publish \
    --no-restore \
    /p:OutputPath=/app/publish



FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine
WORKDIR /app

COPY --from=backend-build /app/publish ./
COPY --from=frontend-build /src/frontend/dist ./wwwroot

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
    CMD wget --no-verbose --tries=1 --spider http://127.0.0.1:8080/health || exit 1

ENTRYPOINT ["dotnet", "GameServer.dll"]
