FROM mcr.microsoft.com/mssql-tools

# Copia o script SQL para dentro do container
COPY init.sql /init.sql

# Aguarda o SQL Server estar no ar e depois executa o script
CMD /bin/bash -c "sleep 30 && /opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P Passw0rd -i /init.sql"
