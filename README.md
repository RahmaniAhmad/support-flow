# SupportFlow

**SupportFlow** is a modern, AI-powered customer support and ticket management platform built with **ASP.NET Core, Next.js, PostgreSQL, and RAG-based AI capabilities**.

It is designed to provide a scalable foundation for managing customer support, tickets, knowledge articles, users, and AI-assisted support workflows.

> **Source-available:** SupportFlow is free for personal, educational, and non-commercial use. Commercial, organizational, and SaaS use requires a separate commercial license.

---

## ✨ Features

### 🎫 Ticket Management

- Create and manage support tickets
- Ticket status workflow
- Ticket assignment
- Ticket filtering and access control
- Ticket history and domain events
- Company-based ticket isolation

### 👥 User & Access Management

- User management
- Role-based authorization
- Permission-based access control
- Company/tenant isolation
- Agent assignment
- User activation/deactivation

### 📚 Knowledge Base

- Create and manage knowledge articles
- Search knowledge articles
- Article-based support knowledge
- Semantic search using vector embeddings

### 🤖 AI & RAG

SupportFlow includes an AI foundation for intelligent customer support:

- Text embeddings
- Vector search
- Semantic knowledge-base search
- Retrieval-Augmented Generation (RAG)
- AI-powered support workflows
- PostgreSQL + pgvector integration
- Gemini embedding integration

The AI architecture is designed to allow support agents to retrieve relevant company knowledge when handling customer requests.

### ⚡ Backend Architecture

- ASP.NET Core
- .NET
- Domain-Driven Design (DDD)
- CQRS
- MediatR
- Domain Events
- Vertical Slice Architecture
- Entity Framework Core
- PostgreSQL
- Redis caching
- RabbitMQ

### 🎨 Frontend

- Next.js
- React
- TypeScript
- Ant Design
- Tailwind CSS
- React Query
- Axios

### 🔐 Security

- JWT authentication
- HttpOnly authentication cookies
- Refresh tokens
- CSRF protection
- Role-based authorization
- Permission-based authorization
- Multi-tenant data isolation

---

## 🏗️ Architecture

SupportFlow is organized into two main applications: a **Backend** built with ASP.NET Core and a **Frontend** built with Next.js.

The backend follows **Domain-Driven Design, CQRS, and Vertical Slice Architecture**.

```text
SupportFlow
│
├── Backend
│   ├── Api
│   ├── Infrastructure
│   └── Shared
│
└── Frontend
```

### Backend

```text
Backend
│
├── Api
│   └── HTTP endpoints and application features
│
├── Infrastructure
│   ├── Persistence
│   ├── Authentication
│   ├── AI
│   ├── Embeddings
│   ├── Caching
│   └── External services
│
└── Shared
    ├── Domain
    ├── Application abstractions
    └── Shared contracts
```

The architecture is designed to keep business rules independent from infrastructure concerns while allowing individual features to evolve independently.

### Frontend

```text
Frontend
│
├── Next.js
├── React
├── Components
├── Features
└── UI
```

---

## 🧠 AI Architecture

SupportFlow uses a vector-based knowledge retrieval pipeline.

```text
Knowledge Article
       │
       ▼
Embedding Service
       │
       ▼
Vector Embedding
       │
       ▼
PostgreSQL + pgvector
       │
       │
       ▼
User Query
       │
       ▼
Query Embedding
       │
       ▼
Semantic Search
       │
       ▼
Relevant Knowledge
       │
       ▼
AI Support Workflow
```

This allows the system to retrieve knowledge based on **meaning rather than simple keyword matching**.

The architecture provides a foundation for building more advanced AI-powered support workflows and RAG-based features.

---

## 🛠️ Technology Stack

| Area            | Technology                  |
| --------------- | --------------------------- |
| Backend         | .NET / ASP.NET Core         |
| Architecture    | DDD / CQRS / Vertical Slice |
| ORM             | Entity Framework Core       |
| Database        | PostgreSQL                  |
| Vector Database | PostgreSQL + pgvector       |
| Cache           | Redis                       |
| Messaging       | RabbitMQ                    |
| Frontend        | Next.js / React             |
| Language        | TypeScript                  |
| UI              | Ant Design / Tailwind CSS   |
| Data Fetching   | React Query                 |
| HTTP Client     | Axios                       |
| AI              | Google Gemini               |
| Embeddings      | Gemini Embeddings           |
| Containers      | Docker                      |
| Version Control | Git                         |

---

## 🚧 Project Status

SupportFlow is currently under active development.

The project is primarily intended as a **source-available reference implementation and personal/non-commercial software project**. Features, APIs, and architecture may evolve as development continues.

It should not currently be considered a fully production-ready commercial support platform without additional configuration, testing, monitoring, and operational hardening.

---

## 🚀 Getting Started

### Prerequisites

Make sure you have the following installed:

- .NET SDK
- Node.js
- Docker
- Docker Compose
- Git

PostgreSQL and other infrastructure services are expected to be provided through the project's Docker configuration.

### Clone the repository

```bash
git clone https://github.com/RahmaniAhmad/support-flow.git

cd SupportFlow
```

### Start infrastructure

```bash
docker compose up -d
```

This starts the infrastructure required by the application, including PostgreSQL and other configured services.

### Backend

Navigate to the backend API:

```bash
cd Backend/Api
```

Restore dependencies:

```bash
dotnet restore
```

Run the API:

```bash
dotnet run
```

### Frontend

Open a new terminal and navigate to the frontend:

```bash
cd Frontend
```

Install dependencies:

```bash
npm install
```

Run the development server:

```bash
npm run dev
```

The frontend will then be available through the configured Next.js development server.

---

## ⚙️ Configuration

Create the appropriate development configuration files for your environment.

Typical configuration includes:

- PostgreSQL connection string
- Redis connection
- RabbitMQ configuration
- JWT configuration
- Authentication settings
- AI provider configuration
- Gemini API credentials
- CORS configuration

### Example

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5433;Database=supportflow;Username=supportflow;Password=supportflow"
  }
}
```

> **Never commit API keys, passwords, JWT secrets, or other credentials to the repository.**

Use environment variables or local development configuration for sensitive values.

---

## 🗄️ Database

SupportFlow uses PostgreSQL as its primary database.

The project also uses **pgvector** for storing and querying vector embeddings.

Make sure the pgvector extension is enabled in your PostgreSQL database before using semantic search functionality.

---

## 🔍 Semantic Search

Knowledge articles can be converted into vector embeddings and stored in PostgreSQL.

When a user performs a semantic search:

1. The query is converted into an embedding.
2. The embedding is compared with stored knowledge vectors.
3. The most relevant knowledge articles are retrieved.
4. The retrieved knowledge can be used by AI-powered support workflows.

This provides the foundation for SupportFlow's RAG capabilities.

---

## 🧪 Development

### Build the backend

```bash
dotnet build
```

### Run backend tests

```bash
dotnet test
```

### Build the frontend

```bash
npm run build
```

### Run the frontend

```bash
npm run dev
```

---

## 🐳 Docker

SupportFlow uses Docker for local infrastructure and development.

### Start services

```bash
docker compose up -d
```

### Stop services

```bash
docker compose down
```

### View running containers

```bash
docker compose ps
```

---

## 🗺️ Roadmap

The project is actively evolving.

Potential future improvements include:

- [ ] Advanced AI ticket suggestions
- [ ] AI-generated ticket summaries
- [ ] AI-assisted responses
- [ ] Improved RAG pipeline
- [ ] AI agent workflows
- [ ] Email integration
- [ ] Notifications
- [ ] Customer portal
- [ ] Advanced reporting
- [ ] Subscription and billing
- [ ] Audit logging
- [ ] API documentation
- [ ] Production deployment documentation

---

## 🤝 Contributions

Personal and non-commercial contributions are welcome.

Before submitting a contribution, please review the project's license and contribution guidelines.

For commercial development, redistribution, or integration into commercial products, please contact the copyright holder before proceeding.

---

## 📜 License

SupportFlow is **source-available software**.

It is free to use for:

- Personal projects
- Learning
- Education
- Individual experimentation
- Non-commercial projects

Commercial and organizational use requires a separate commercial license.

Commercial use includes:

- Using SupportFlow within a company or organization
- Deploying SupportFlow for business operations
- Providing SupportFlow as a service
- Offering SupportFlow as SaaS
- Selling or sublicensing SupportFlow
- Incorporating SupportFlow into a commercial product
- Providing SupportFlow-based services to customers

See the [`LICENSE`](./LICENSE) file for the complete license terms.

### Commercial Licensing

If you are interested in using SupportFlow commercially, please contact:

**Ahmad Rahmani**

Commercial licensing terms and fees are determined separately.

---

## ⭐ Support the Project

If you find SupportFlow useful for learning or personal projects, consider giving the repository a ⭐ on GitHub.

---

## 👨‍💻 Author

**Ahmad Rahmani**

Full-Stack Developer focused on backend engineering, distributed systems, DDD, CQRS, and AI-powered applications.

### Built with

- .NET
- ASP.NET Core
- React
- Next.js
- PostgreSQL
- Redis
- RabbitMQ
- RAG
- Vector Search
- AI
