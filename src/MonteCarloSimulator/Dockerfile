FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MonteCarloSimulator/MonteCarloSimulator.csproj", "MonteCarloSimulator/"]
RUN dotnet restore "MonteCarloSimulator/MonteCarloSimulator.csproj"
COPY . .
WORKDIR "/src/MonteCarloSimulator"
RUN dotnet build "MonteCarloSimulator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MonteCarloSimulator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MonteCarloSimulator.dll"]
