FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PoolBinanceMonitor.csproj", "./"]
RUN dotnet restore "./PoolBinanceMonitor.csproj"
COPY . .
RUN dotnet publish "PoolBinanceMonitor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "PoolBinanceMonitor.dll"]
