# Lead Manager Full Stack

Aplicação full-stack para gerenciar **leads**, composta por:

- **Backend**: ASP.NET Core  + Entity Framework Core + SQL Server  
- **Frontend**: React + Vite + TypeScript  
- **DB**: SQL Server (recomendado via Docker)

---

## Sumário

1. [Requisitos](#requisitos)  
2. [Estrutura do Projeto](#estrutura-do-projeto)  
3. [Configuração do Banco (Docker)](#configuração-do-banco-docker)  
4. [Backend (.NET)](#backend-net)  
5. [Frontend (React + Vite)](#frontend-react--vite)  
6. [Rodando front e back juntos (opcional)](#rodando-front-e-back-juntos-opcional)  
7. [Endpoints principais](#endpoints-principais)  
8. [Seed automático e desconto](#seed-automático-e-desconto)  
9. [Estilos (CSS) — convenções](#estilos-css--convenções)  
10. [Dicas de troubleshooting](#dicas-de-troubleshooting)

---

## Requisitos

- **Node.js** 18+ e **npm** 9+  
- **.NET SDK** 8.0+
- **Docker Desktop** (para subir o SQL Server rapidamente)

> ⚠️ No Windows PowerShell, habilite scripts do npm se necessário:
> ```powershell
> Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy RemoteSigned
> ```

---

## Estrutura do Projeto

```
desafio-fullstack/
├─ backend/                         # API .NET 8
│  ├─ Controllers/
│  │  └─ LeadsController.cs
│  ├─ Data/
│  │  ├─ AppDbContext.cs
│  │  └─ SeedHelpers.cs
│  ├─ Migrations/
│  ├─ Models/
│  │  └─ Lead.cs
│  ├─ Services/
│  │  └─ FileEmailFake.cs           # “envio de email” fake (gera arquivo em backend/outbox)
│  ├─ appsettings.json
│  ├─ Program.cs
│  └─ LeadsApi.csproj
│
├─ frontend/                        # React + Vite + TS
│  ├─ src/
│  │  ├─ api/
│  │  │  └─ api.ts                  # Base URL da API
│  │  ├─ components/
│  │  │  ├─ LeadCard.tsx
│  │  │  └─ (outros componentes)
│  │  ├─ styles/                    # CSS modularizado
│  │  │  ├─ tokens.css              # variáveis de tema
│  │  │  ├─ base.css                # resets / estilos globais
│  │  │  ├─ layout.css              # containers, grid, alinhamentos
│  │  │  ├─ utilities.css           # utilitárias (helpers)
│  │  │  ├─ tabs.css                # estilos das abas
│  │  │  ├─ card.css                # estilos de cards
│  │  │  ├─ buttons.css             # estilos de botões
│  │  │  └─ main.css                # arquivo agregador 
│  │  ├─ types/
│  │  │  └─ types.ts
│  │  ├─ App.tsx
│  │  ├─ main.tsx
│  │  └─ index.css                  
│  ├─ index.html
│  ├─ package.json
│  └─ vite.config.ts
│
└─ README.md                        # este arquivo
```

---

## Configuração do Banco (Docker)

Suba um SQL Server local em Docker:

```bash
docker run -e "ACCEPT_EULA=Y"   -e "MSSQL_SA_PASSWORD=YourStrong!Passw0rd"   -p 1433:1433 --name mssql2022 -d mcr.microsoft.com/mssql/server:2022-latest
```

Verifique os logs (opcional):

```bash
docker logs -f mssql2022    # aguarde "SQL Server is now ready for client connections"
```

Teste a porta:

```powershell
Test-NetConnection -ComputerName localhost -Port 1433
# TcpTestSucceeded : True
```

---

## Backend (.NET)

1) **Configure a conexão** em `backend/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost,1433;Database=LeadsDb;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=true"
  }
}
```

2) **Restaurar & Rodar**:

```bash
cd backend
dotnet restore
dotnet build
dotnet watch run --urls http://localhost:5080
```

- Swagger: `http://localhost:5080/swagger`
- A API, ao iniciar, **cria o banco (EnsureCreated)** e **povoa 30 leads novos** (seed) se o banco estiver vazio (veja seção [Seed automático](#seed-automático-e-desconto)).

> Se preferir migrations: `dotnet ef database update` (já existem migrações na pasta *Migrations*).

---

## Frontend (React + Vite)

1) Ajuste a **base URL da API** se necessário em `frontend/src/api/api.ts`:
```ts
export const API_BASE = 'http://localhost:5080/api';
```

2) Instale e rode:
```bash
cd frontend
npm install
npm run dev
```

- Vite em: `http://localhost:5173`

---


## Endpoints principais

- `GET /api/leads?status=New|Accepted|Declined` – lista leads por status  
- `POST /api/leads/seed?count=50` – cria *N* leads aleatórios (opcional; a aplicação já povoa sozinha se vazio)
- `POST /api/leads/{id}/accept` – aceita lead; se `price > 500`, aplica **10% de desconto**  
  Também grava um “email fake” em `backend/outbox/`
- `POST /api/leads/{id}/decline` – recusa lead

---

## Seed automático e desconto

- No **startup do backend** (em `Program.cs` + `SeedHelpers.cs`), se o banco estiver **sem registros**, são criados **30 leads** com dados completos (nome, telefone, email, bairro, categoria, descrição, preço, data).  
- Ao **aceitar** um lead:
  - Se `price > 500`, aplica-se **10% de desconto** (arredondado com 2 casas).
  - É gerado um arquivo `.txt` em `backend/outbox/` simulando o envio de e-mail.

---

## Estilos (CSS) — convenções

- CSS modularizado em `frontend/src/styles/`
  - `tokens.css`: variáveis de cor, espaçamento, radius etc.  
  - `base.css`: reset/normalização + estilos globais do `<body/>`.  
  - `layout.css`: `.container`, alinhamento central e responsivo.  
  - `utilities.css`: helpers (ex.: `.text-center`, `.mt-2`).  
  - `tabs.css`, `card.css`, `buttons.css`: camadas por componente/feature.  
  - `main.css`: **agregador** que importa todos os demais.
- `index.css` importa `styles/main.css` – o React só vê **um** arquivo global.

**Container centralizado**: o wrapper `.container` está com largura fixa centrada, e os cards têm largura máxima para manter a leitura confortável em telas grandes (sem “colar” à esquerda).

---

## Dicas de troubleshooting

- **Porta 5080 ocupada**  
  ```powershell
  netstat -ano | findstr :5080
  taskkill /PID <PID> /F
  ```
- **“Conexão recusada” no SQL**  
  - Confirme o container: `docker ps`  
  - Veja logs: `docker logs -f mssql2022`  
  - Teste porta: `Test-NetConnection -ComputerName localhost -Port 1433` deve retornar `TcpTestSucceeded : True`  
  - Verifique `appsettings.json` (senha, porta, `TrustServerCertificate=true`).
- **Frontend não carrega**  
  - Garanta que a API está em `http://localhost:5080`.  
  - Confira `frontend/src/api/api.ts`.  
  - Rode `npm run dev` dentro de `frontend/`.

---
