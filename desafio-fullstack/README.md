# Lead Manager — SPA + .NET 8 Web API + SQL Server

Aplicação full-stack para **gerenciamento de leads** com:
- **Frontend**: React + Vite (TypeScript), SPA com abas **Invited** (New) e **Accepted**
- **Backend**: .NET 8 Web API + **EF Core** (SQL Server)
- **Banco**: SQL Server (via **Docker** ou instalação local)
- **E-mail fake**: grava um `.txt` em `backend/outbox` ao aceitar um lead

---

## Sumário
- [Pré-requisitos](#pré-requisitos)
- [Arquitetura & Funcionalidades](#arquitetura--funcionalidades)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Setup Rápido (TL;DR)](#setup-rápido-tldr)
- [Backend — .NET API](#backend--net-api)
  - [Configurar conexão com o SQL](#configurar-conexão-com-o-sql)
  - [Rodar migrações e API](#rodar-migrações-e-api)
  - [Endpoints](#endpoints)
- [Frontend — React + Vite](#frontend--react--vite)
- [Testes rápidos](#testes-rápidos)
- [Dicas & Solução de Problemas](#dicas--solução-de-problemas)

---

## Pré-requisitos

- **.NET SDK 8**: <https://dotnet.microsoft.com/download>
- **Node.js LTS + npm**: <https://nodejs.org/>
- **SQL Server** de uma das formas:
  - **(Recomendado)** **Docker Desktop**: <https://www.docker.com/products/docker-desktop/>
  - ou **SQL Server Express/Developer** instalado localmente

> **Windows (WSL2)**: se usar Docker, ative **WSL2** e, no Docker Desktop, habilite “Use the WSL 2 based engine” e a integração com sua distro.

---

## Arquitetura & Funcionalidades

- Aba **Invited** lista leads com `status = New` e mostra botões **Accept** / **Decline**.
  - **Accept**: se `price > 500`, aplica **10% de desconto**, muda `status` para **Accepted** e grava um “e-mail” fake em `backend/outbox/…txt`.
  - **Decline**: muda `status` para **Declined`.
- Aba **Accepted** lista leads aceitos e exibe campos extras: **nome completo**, **telefone**, **email**.
- API serializa o enum de status como **string**: `"New" | "Accepted" | "Declined"`.

---

## Estrutura do Projeto

```
desafio-fullstack/
├─ backend/                 # .NET 8 Web API + EF Core
│  ├─ Controllers/
│  │  └─ LeadsController.cs
│  ├─ Data/
│  │  ├─ AppDbContext.cs
│  │  └─ (Migrations…)
│  ├─ Models/
│  │  └─ Lead.cs
│  ├─ Services/
│  │  ├─ IEmailFake.cs
│  │  └─ FileEmailFake.cs   # cria .txt em backend/outbox ao aceitar lead
│  ├─ appsettings.json
│  └─ Program.cs
└─ frontend/                # React + Vite (TypeScript)
   ├─ src/
   │  ├─ App.tsx
   │  ├─ App.css
   │  ├─ LeadCard.tsx
   │  ├─ api.ts
   │  └─ types.ts
   ├─ index.html
   └─ vite.config.ts
```

---

## Setup Rápido (TL;DR)

1) **Subir SQL Server (Docker):**
```powershell
docker run --name mssql2022 -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Your_password123" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

2) **Backend:**
```powershell
cd backend
# ajuste appsettings.json conforme abaixo
dotnet ef database update
dotnet run --urls http://localhost:5080
# Swagger em: http://localhost:5080/swagger
```

3) **Frontend:**
```powershell
cd ../frontend
npm install
npm run dev
# Vite em: http://localhost:5173
```

---

## Backend — .NET API

### Configurar conexão com o SQL

**Opção A — Docker (recomendada):** em `backend/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost,1433;Database=LeadsDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning" }
  },
  "AllowedHosts": "*"
}
```

> Para porta externa diferente, mapeie `-p 1434:1433` e use `Server=localhost,1434`.

**Opção B — SQL Express local:**
```json
"Default": "Server=.\SQLEXPRESS;Database=LeadsDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

**Program.cs — JSON de enums como string e CORS:**
```csharp
builder.Services.AddControllers()
  .AddJsonOptions(o =>
  {
    o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
  });

builder.Services.AddCors(opt =>
{
  opt.AddPolicy("Spa", p =>
    p.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod());
});
```

### Rodar migrações e API
```powershell
cd backend
dotnet tool install --global dotnet-ef  # se ainda não tiver
dotnet ef database update               # cria o banco + seed inicial
dotnet run --urls http://localhost:5080
```

**Swagger:** <http://localhost:5080/swagger>

### Endpoints
- `GET /api/leads?status=New` — lista “Invited”  
- `GET /api/leads?status=Accepted` — lista “Accepted”  
- `POST /api/leads/{id}/accept` — aceita (aplica 10% se `price > 500`) e cria outbox `.txt`  
- `POST /api/leads/{id}/decline` — recusa

---

## Frontend — React + Vite

```powershell
cd frontend
npm install
npm run dev
```

Abra <http://localhost:5173>.

- Abas **Invited**/**Accepted**.
- Ao **Accept/Decline**, a lista **Invited** é recarregada automaticamente.

---

## Testes rápidos

**Checar leads New (Invited):**
```
GET http://localhost:5080/api/leads?status=New
```

**Aceitar o lead 1 (aplica 10% se > 500 e cria .txt em backend/outbox):**
```
POST http://localhost:5080/api/leads/1/accept
```

**Recusar o lead 2:**
```
POST http://localhost:5080/api/leads/2/decline
```

**Ver Accepted:**
```
GET http://localhost:5080/api/leads?status=Accepted
```

---

## Dicas & Solução de Problemas

- **Docker não reconhecido**: instale Docker Desktop, reinicie, e verifique `docker version`.  
- **Erro 500 / _ping no Docker**: habilite WSL2, integre o Docker com sua distro (Docker Desktop → Settings → Resources → WSL integration).  
- **LocalDB não encontrado**: use Docker (acima) ou instale SQL Express/Developer.  
- **Porta ocupada**: mapeie outra porta `-p 1434:1433` e ajuste a connection string.  
- **Enum vindo como número (`0/1/2`)**: garanta `JsonStringEnumConverter` no `Program.cs`.  
- **CORS**: confira a policy permitindo `http://localhost:5173`.  
- **Build travado no Windows (arquivo em uso)**:
  ```powershell
  taskkill /IM LeadsApi.exe /F
  taskkill /IM dotnet.exe /F
  dotnet build-server shutdown
  dotnet clean
  ```
- **Resetar base e dados de seed**:
  ```powershell
  dotnet ef database drop -f
  dotnet ef database update
  ```

---

### Segurança (desenvolvimento vs produção)

- **Dev local**: ok usar a senha `Your_password123` no `appsettings.json`.  
- **Produção**:
  - Use senha forte e **variáveis de ambiente**/secret manager.
  - Não exponha o SQL Server publicamente.
  - Considere migrations com pipeline e backups.
