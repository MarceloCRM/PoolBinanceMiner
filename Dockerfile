# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o csproj e restaura dependências
COPY *.csproj ./
RUN dotnet restore

# Copia o restante do código e compila
COPY . .
RUN dotnet publish -c Release -o /app

# Etapa final (imagem menor)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .

# Expõe a porta usada pela aplicação
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "PoolBinanceMonitor.dll"]
