# Rh-Backend

API backend desenvolvida em .NET 9 para gestão de funcionários, cargos, contratos, férias e histórico de alterações.

---

## Pré-Requisitos

Antes de rodar o projeto, certifique-se de ter instalado:

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)(opcional)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)(opcional) (Express ou versão completa)
- [Docker](https://www.docker.com/get-started) (recomendavel, para execução em containers)
- IDE de sua preferência (Visual Studio, Visual Studio Code, Rider, etc.)

---

## Instruções para Rodar o Sistema Localmente

### 1. Configuração do Banco de Dados

- Instale e configure o SQL Server localmente.
- Execute o script SQL para criação do banco e tabelas, localizado em `init.sql`.

### 2. Restaurar e Executar a API

- Abra o terminal na raiz do projeto e execute:

```bash
dotnet restore
dotnet build
dotnet run --project Rh-Backend/Rh-Backend.csproj
```

- A aplicação estará disponível em `http://localhost:5234`

---

## Rodando com Docker e Docker Compose (Recomendado)

Para rodar o banco de dados e a API usando Docker, execute:

```bash
docker-compose up --build
```

- O SQL Server estará rodando com o banco configurado.
- A API será exposta na porta 5234.

---

## Considerações sobre o Banco de Dados

- Usuário de conexão: `adminRh` com senha `123` (uso para ambiente de desenvolvimento).
- As tabelas são criadas conforme o script disponibilizado no repositório.
- O campo `status` da tabela `ferias` é restrito aos valores `'Pendente', 'Em andamento', 'Concluídas'` via constraint CHECK.
- Certifique-se de que o SQL Server permite conexões TCP na porta configurada.

---

## Endpoint do Swagger para Teste da API

Após iniciar a aplicação, acesse o Swagger UI para testar a API em:

```
http://localhost:5234/swagger/index.html
```

---
