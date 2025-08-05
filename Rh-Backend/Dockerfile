# Base com ASP.NET Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER root
WORKDIR /app
EXPOSE 5234

# SDK com ferramentas de desenvolvimento
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev
WORKDIR /src

# Copia apenas o csproj e restaura as dependências
COPY ["Rh-Backend/Rh-Backend.csproj", "Rh-Backend/"]
RUN dotnet restore "Rh-Backend/Rh-Backend.csproj"

# Copia o restante dos arquivos
COPY . .

WORKDIR "/src/Rh-Backend"

# Instala o dotnet-watch globalmente
RUN dotnet tool install --global dotnet-watch

# Adiciona o caminho das ferramentas ao PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Usa dotnet watch para hot reload no desenvolvimento
ENTRYPOINT ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:5234"]
