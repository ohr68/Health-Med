FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/HealthMed.ApiGateway/HealthMed.ApiGateway.csproj", "src/HealthMed.ApiGateway/"]
COPY ["src/HealthMed.Common/HealthMed.Common.csproj", "src/HealthMed.Common/"]
COPY ["src/HealthMed.Domain/HealthMed.Domain.csproj", "src/HealthMed.Domain/"]
RUN dotnet restore "src/HealthMed.ApiGateway/HealthMed.ApiGateway.csproj"
COPY . .
WORKDIR "/src/src/HealthMed.ApiGateway"
RUN dotnet build "HealthMed.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HealthMed.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthMed.ApiGateway.dll"]
