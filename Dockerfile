# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files first to leverage layer caching on restore.
COPY SweetMedical.slnx ./
COPY src/SweetMedical.Domain/SweetMedical.Domain.csproj src/SweetMedical.Domain/
COPY src/SweetMedical.Application/SweetMedical.Application.csproj src/SweetMedical.Application/
COPY src/SweetMedical.Contracts/SweetMedical.Contracts.csproj src/SweetMedical.Contracts/
COPY src/SweetMedical.Infrastructure/SweetMedical.Infrastructure.csproj src/SweetMedical.Infrastructure/
COPY src/SweetMedical.Api/SweetMedical.Api.csproj src/SweetMedical.Api/
COPY tests/SweetMedical.Tests/SweetMedical.Tests.csproj tests/SweetMedical.Tests/

RUN dotnet restore SweetMedical.slnx

# Copy the rest of the source and publish the API.
COPY . .
RUN dotnet publish src/SweetMedical.Api/SweetMedical.Api.csproj -c Release -o /app/publish --no-restore

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080

ENTRYPOINT ["dotnet", "SweetMedical.Api.dll"]
