# 🧾 Rh-Backend (.NET 9)

API backend desenvolvida em .NET 9 para gestão de funcionários, cargos, contratos, férias e histórico de alterações.

---

## 🚀 Tecnologias Utilizadas

- 🧠 **Backend**: ASP.NET Core (.NET 9)
- 🐘 **Banco de Dados**: SQL Server
- 🐋 **Containerização**: Docker & Docker Compose

---

## ⚙️ Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) (opcional)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (opcional)
- [Docker](https://www.docker.com/get-started) (recomendado)

---

## 💻 Rodando localmente com Docker

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/rh-backend.git
cd rh-backend
```

### 2. Configure o ambiente

- Configure suas variáveis de ambiente ou use as padrões no `docker-compose.yml`.
- Certifique-se de que as portas `5234` (API) e `1433` (SQL Server) estejam livres.

### 3. Execute com Docker

```bash
docker-compose up --build
```

Esse comando irá:

- Subir o SQL Server com os dados iniciais definidos em `init.sql`
- Buildar e rodar a API na porta `5234`

---

### 4. Acesse a aplicação

- **Swagger da API**: http://localhost:5234/swagger/index.html

---

## 🗃️ Acesso ao Banco de Dados

- **Usuário**: `adminRh`  
- **Senha**: `123`  
- O banco e as tabelas são criados automaticamente via script `init.sql`.

---

## 🧪 Testes

O projeto inclui **5 testes automatizados**, um para cada classe principal da aplicação (`Funcionário`, `Cargo`, `Contrato`, `Férias` e `Histórico`).  
Eles estão organizados na pasta de testes e podem ser executados com:

```bash
dotnet test
```

---

## 🧪 Executar localmente sem Docker

Se preferir executar diretamente via .NET CLI:

```bash
dotnet restore
dotnet build
dotnet run --project Rh-Backend/Rh-Backend.csproj
```

A API estará acessível em `http://localhost:5234`

---

## 🧱 Estrutura do Projeto

```
/Rh-Backend
  ├── Controllers/
  ├── Models/
  ├── Services/
  ├── Data/
  ├── Tests/
  ├── init.sql
  └── Program.cs
```