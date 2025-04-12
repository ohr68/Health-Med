FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/HealthMed.AuthApi/HealthMed.AuthApi.csproj", "src/HealthMed.AuthApi/"]
COPY ["src/HealthMed.Common/HealthMed.Common.csproj", "src/HealthMed.Common/"]
COPY ["src/HealthMed.Domain/HealthMed.Domain.csproj", "src/HealthMed.Domain/"]
COPY ["src/HealthMed.Keycloak/HealthMed.Keycloak.csproj", "src/HealthMed.Keycloak/"]
RUN dotnet restore "src/HealthMed.AuthApi/HealthMed.AuthApi.csproj"
COPY . .
WORKDIR "/src/src/HealthMed.AuthApi"
RUN dotnet build "HealthMed.AuthApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HealthMed.AuthApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthMed.AuthApi.dll"]
