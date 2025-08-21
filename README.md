[![CodeQL Advanced](https://github.com/AugustoRengel/script-hive/actions/workflows/codeql.yml/badge.svg)](https://github.com/AugustoRengel/script-hive/actions/workflows/codeql.yml)
 
 # 🛠️ script-hive

**ScriptHive** é uma plataforma de execução e gestão de scripts, 
projetada para automatizar transformações de dados de forma segura e escalável. 
O sistema permite armazenar scripts em JavaScript, executá-los de maneira assíncrona, 
acompanhar suas execuções e persistir os resultados em um banco de dados.

---

## 📌 Tecnologias e Arquitetura

- **.NET 9**
- **Entity Framework Core**
- **PostgreSQL (Docker)**
- **Swagger UI**
- **JWT Authentication**
- **Arquitetura DDD (Domain-Driven Design)**
- **API REST**
- **Docker + Docker Compose**

---
## 🚀 Como rodar o projeto (Git Bash + Docker)

### ⚙️ Pré-requisitos

- Docker Desktop instalado e rodando
- Git Bash (ou terminal compatível com Unix-like shell no Windows)

---

### 🧾 1. Clone o repositório

```bash
git clone git@github.com:AugustoRengel/script-hive.git
cd script-hive
```

---

### 🐳 2. Suba os containers

```bash
docker compose up --build
```

Este comando irá:
- Construir a imagem da API
- Criar o banco `script_hive` (PostgreSQL)
- Aplicar as **migrations**
- Popular a tabela User um um Admin
- Aguardar o banco estar pronto e conectar a API
- Iniciar a API na porta `http://localhost:5351`

---

### 🔎 3. Acesse o Swagger

Após os containers subirem, acesse:

```
http://localhost:5351/swagger
```

---

### 🔐 4. Login com JWT (para acessar endpoints protegidos)

1. Acesse `/auth/login`
2. Use as credenciais geradas pelo seed:

```json
{
  "username": "Admin",
  "password": "admin@123"
}
```

3. Copie o token JWT retornado
4. No Swagger, clique em **Authorize** e cole o token (sem `Bearer`)

---

### 🧼 Resetar o ambiente (limpar banco e containers)

```bash
docker-compose down --volumes
```

Esse comando remove os containers **e o volume do banco**, permitindo um ambiente novo ao rodar `up` novamente.

---

## 🧪 Testes

Os testes unitários podem ser executados com:

```bash
dotnet test .\ScriptHive.sln
```

Requer .NET SDK 9 localmente instalado.

---

## 🧱 Estrutura do Projeto

```
script-hive/
│
├── src/ 
│	├── ScriptHive.Api				# Camada de apresentação com endpoints REST, injeção de dependências e autenticação JWT
│	├── ScriptHive.Application		# Contém casos de uso, DTOs, validações e interfaces de serviços.
│	├── ScriptHive.Domain			# Contém entidades, value objects e interfaces de repositórios e filas.
│	├── ScriptHive.Infrastructure	# Contém implementações de repositórios, contexto EF e implementação da fila em memória.
│	├── ScriptHive.Worker			# Worker que atua em background para persistir os resultados dos scripts executados.
│	└── ScriptHive.ScriptExecutor	# Execução de scripts .js com Jint
├── tests 
│	└── ScriptHive.Tests			# Testes unitários
├── docker-compose.yml				# Orquestração dos containers
└── README.md						# Este arquivo
```

---

## 👨‍💻 Autenticação

- Baseada em **JWT**
- Login no endpoint `/auth/login`
- Autorização via `[Authorize]` nos endpoints protegidos

---

## 🧠 DDD aplicado

- Separação clara entre camadas: `Api`, `Application`, `Domain` e `Infrastructure`
- Uso de DTOs para entrada e saída
- Services lidam com regras e validações, respeitando princípios **SOLID**

---

## 📂 Endpoints

- `/auth` – Login, geração de token - Public
- `/users` – Gerenciar users (CRUD) - Admin only
- `/scripts` – Gerenciar scripts (CRUD) - Admin and User
- `/executions` – Executar scripts e acompanhar status/resultados - Admin and User
