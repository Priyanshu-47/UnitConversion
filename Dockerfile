# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore first for better layer caching.
COPY Directory.Build.props ./
COPY global.json ./
COPY src/UnitConversion.Core/UnitConversion.Core.csproj src/UnitConversion.Core/
COPY src/UnitConversion.Api/UnitConversion.Api.csproj src/UnitConversion.Api/
COPY tests/UnitConversion.Tests/UnitConversion.Tests.csproj tests/UnitConversion.Tests/
RUN dotnet restore src/UnitConversion.Api/UnitConversion.Api.csproj

COPY . .
RUN dotnet publish src/UnitConversion.Api/UnitConversion.Api.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "UnitConversion.Api.dll"]
