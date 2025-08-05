# Use a imagem oficial do SQL Server 2022 para Linux
FROM mcr.microsoft.com/mssql/server:2022-latest

# Define as variáveis de ambiente exigidas pelo SQL Server
ENV SA_PASSWORD=Passw0rd \
    ACCEPT_EULA=Y

# Expõe a porta padrão do SQL Server
EXPOSE 1433

# Comando padrão para iniciar o SQL Server
CMD ["/opt/mssql/bin/sqlservr"]
