# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["B3.Invest/B3.Invest.csproj", "B3.Invest/"]
COPY ["B3.Invest.Application/B3.Invest.Application.csproj", "B3.Invest.Application/"]
COPY ["B3.Invest.Domain/B3.Invest.Domain.csproj", "B3.Invest.Domain/"]
RUN dotnet restore "./B3.Invest/B3.Invest.csproj"
COPY . .
WORKDIR "/src/B3.Invest"
RUN dotnet build "./B3.Invest.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./B3.Invest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "B3.Invest.dll"]