FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/HealthMed.WebApi/HealthMed.WebApi.csproj", "src/HealthMed.WebApi/"]
COPY ["src/HealthMed.Application/HealthMed.Application.csproj", "src/HealthMed.Application/"]
COPY ["src/HealthMed.Domain/HealthMed.Domain.csproj", "src/HealthMed.Domain/"]
COPY ["src/HealthMed.ORM/HealthMed.ORM.csproj", "src/HealthMed.ORM/"]
COPY ["src/HealthMed.Keycloak/HealthMed.Keycloak.csproj", "src/HealthMed.Keycloak/"]
COPY ["src/HealthMed.Caching/HealthMed.Caching.csproj", "src/HealthMed.Caching/"]
COPY ["src/HealthMed.Common/HealthMed.Common.csproj", "src/HealthMed.Common/"]
COPY ["src/HealthMed.Ioc/HealthMed.Ioc.csproj", "src/HealthMed.Ioc/"]
COPY ["src/HealthMed.Messaging/HealthMed.Messaging.csproj", "src/HealthMed.Messaging/"]
RUN dotnet restore "src/HealthMed.WebApi/HealthMed.WebApi.csproj"
COPY . .
WORKDIR "/src/src/HealthMed.WebApi"
RUN dotnet build "HealthMed.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HealthMed.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthMed.WebApi.dll"]
