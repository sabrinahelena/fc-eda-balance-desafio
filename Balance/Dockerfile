# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["Balance.API/Balance.API.csproj", "Balance.API/"]
COPY ["Balance.Application/Balance.Application.csproj", "Balance.Application/"]
COPY ["Balance.Domain/Balance.Domain.csproj", "Balance.Domain/"]
COPY ["Balance.Infra/Balance.Infra.csproj", "Balance.Infra/"]
RUN dotnet restore "Balance.API/Balance.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/app/Balance.API"
RUN dotnet build "Balance.API.csproj" -c Release -o /app/build
RUN dotnet publish "Balance.API.csproj" -c Release -o /app/publish

# Stage 2: Serve
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Balance.API.dll"]
