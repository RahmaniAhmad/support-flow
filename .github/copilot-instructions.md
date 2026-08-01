# VSA Tickets - GitHub Copilot Instructions

You are working on **VSA Tickets**, a ticket management system.

Always follow the existing architecture, coding patterns, naming conventions, and business rules described below.

---

# Technology Stack

## Backend

- ASP.NET Core .NET 9 Minimal API
- Clean Architecture
- Domain-Driven Design (DDD)
- CQRS with MediatR
- Entity Framework Core
- PostgreSQL
- Redis caching
- Domain Events
- Fluent Validation
- Feature-based architecture

## Frontend

- Next.js App Router
- React
- TypeScript
- Tailwind CSS
- Ant Design
- React Query for server state management
- React Hook Form
- Zod validation

---

# Backend Architecture

The solution follows Clean Architecture and feature-based organization.

Structure:

Api
├── Features
│ ├── Tickets
│ ├── Users
│ └── Authentication
├── Authorization
└── Endpoints

Infrastructure
├── Persistence
├── EF Core Configurations
└── External Services

Shared
├── Contracts
├── Domain Entities
├── Enums
└── Shared Services

Use feature folders.

Example:

Features
└── Tickets
└── CreateTicket
├── CreateTicketCommand.cs
├── CreateTicketCommandHandler.cs
└── CreateTicketEndpoint.cs

---

# Backend Coding Rules

Follow these rules:

- Use CQRS with MediatR.
- Commands modify state.
- Queries only read data.
- Use records for commands, queries, and response models.
- Use async methods with CancellationToken.
- Keep handlers focused on one responsibility.
- Keep endpoints thin.
- Do not put business logic inside endpoints.
- Keep business rules inside domain entities or domain services.
- Avoid unnecessary abstractions.
- Do not create repositories unless there is a real domain requirement.

Endpoints should only:

1. Receive requests.
2. Create commands or queries.
3. Send through MediatR.
4. Return results.

Example:

```csharp
app.MapGet(
    "/tickets",
    async (
        [AsParameters] GetTicketsRequest request,
        ISender sender,
        CancellationToken cancellationToken) =>
{
    var result = await sender.Send(
        new GetTicketsQuery(
            request.Page,
            request.PageSize),
        cancellationToken);

    return Results.Ok(result);
});
```

Domain Rules
Ticket Entity

Main fields:

Id
TicketNumber
Subject
Description
Status
CompanyId
CreatedByUserId
AssignedToUserId
CreatedAtUtc
UpdatedAtUtc

Ticket Status
Open
Assigned
InProgress
Resolved
Closed
Reopened

Ticket state changes must happen through commands.

Examples:

AssignTicketCommand
ResolveTicketCommand
CloseTicketCommand
ReopenTicketCommand
AddCommentCommand

Authorization
The system uses permission-based authorization.

Do not rely only on roles.

Roles

SuperAdmin
Platform-level user.
Can access all companies.
Used for system management.

Admin
Belongs to a company.
Manages company users.
Can view company tickets.
Can assign tickets.

Agent
Belongs to a company.
Works on company tickets.
Can update tickets based on permissions.

Customer
Creates tickets.
Can only access tickets they created.

Always check permissions before adding features.

Example permissions:

DashboardView

TicketsView

UsersView
UsersCreate
UsersUpdate
UsersChangeRole

Ticket Listing Design

Tickets are a single resource.

Do not create separate pages for:

My Tickets
Assigned Tickets
Created Tickets

These are filters.

Use:

GET /tickets?view=All

GET /tickets?view=AssignedToMe

GET /tickets?view=CreatedByMe

The ticket query supports:

Page
PageSize
Search
Status
View
SortBy
Descending

Frontend Architecture

Use feature-based organization.

Example:

features
└── tickets
├── components
│ ├── TicketList.tsx
│ ├── TicketFilters.tsx
│ ├── TicketActions.tsx
│ └── TicketTableColumns.tsx
│
├── hooks
│ └── useTickets.ts
│
├── api
│ └── tickets.ts
│
└── types.ts
Frontend Rules

Follow these rules:

API calls belong in service files.
React Query logic belongs in hooks.
Components should focus on UI.
Reuse existing components.
Avoid duplicated components.
Keep business logic out of UI components.

Use:

React Query for server state.
React Hook Form for forms.
Zod for validation.
Ant Design for complex UI components.
Tailwind CSS for layout and styling.

Example:

useTickets()

should handle:

Fetching data.
Loading state.
Errors.
Cache updates.

Components should not directly call APIs.

Authentication and State

Authentication uses:

HttpOnly cookies.
JWT access token.
Refresh token flow.

Do not store authentication tokens in localStorage.

Use existing authentication providers and hooks.

Caching

Redis is used for caching.

Use:

Cache keys.
Cache groups.
Cache versioning.
Cache invalidation pipeline.

Commands that modify data should invalidate related cache groups.

Do not manually clear cache inside handlers.

Use the existing cache invalidation behavior.

Naming Conventions
Backend
CreateTicketCommand
CreateTicketCommandHandler
CreateTicketEndpoint

GetTicketsQuery
GetTicketsQueryHandler
GetTicketsEndpoint
Frontend
useTickets

ticketService

TicketList

TicketFilters

Prefer clear and descriptive names.

Code Quality Rules

Before creating new code:

Check existing patterns.
Reuse existing components and services.
Follow existing naming conventions.
Consider authorization requirements.
Consider whether something is a new resource or only a filter.

Avoid:

Adding libraries without asking.
Creating unnecessary abstractions.
Creating duplicate components.
Putting business rules in endpoints.
Bypassing authorization.
Creating separate pages when a filter is enough.
Mixing API calls with UI components.
Ignoring existing project conventions.

```

```
