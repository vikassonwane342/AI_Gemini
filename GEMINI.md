# Project Overview: Car Marketplace API

The **Car Marketplace API** is a robust, production-ready .NET 10 Web API designed to facilitate a marketplace for buying and selling cars. It supports user authentication, car listing management, and order processing.

## Key Technologies
- **Framework:** .NET 10 (Web API)
- **Data Access:** [Dapper](https://github.com/DapperLib/Dapper) (Micro-ORM)
- **Database:** Microsoft SQL Server
- **Authentication:** JWT Bearer (JSON Web Tokens)
- **API Documentation:** Swagger/OpenAPI (Swashbuckle)
- **Programming Language:** C# 14 (inferred from .NET 10)

## Architecture & Layers
The project follows a **Clean Architecture** pattern, as detailed in `docs/architecture.md`:
1.  **API Layer (`CarMarketplace.Api/Controllers/`):** Handles HTTP requests and thin controllers.
2.  **Application Layer (`CarMarketplace.Api/Services/`, `CarMarketplace.Api/Dtos/`):** Contains business logic, services, and DTOs.
3.  **Domain Layer (`CarMarketplace.Api/Entities/`):** Core business rules and entities.
4.  **Infrastructure Layer (`CarMarketplace.Api/Repositories/`):** Database access using Dapper and raw SQL.

**Request Flow:** `Request → Controller → Service → Repository → Database`

## Building and Running

### Prerequisites
- .NET 10 SDK
- SQL Server (LocalDB or full instance)

### Database Setup
1.  Ensure SQL Server is running.
2.  Execute the `script.sql` file in the root to create `CarMarketplaceDb` and seed data.
3.  Verify the connection string in `CarMarketplace.Api/appsettings.json`.

### Commands
- **Restore & Build:** `dotnet build`
- **Run the API:** `dotnet run --project CarMarketplace.Api`
- **Access Swagger UI:** `https://localhost:<port>/swagger`

## Development Conventions

Refer to `docs/convensions.md` and `docs/architecture.md` for full details:
- **Clean Code:** Follow SOLID principles, keep methods small, and avoid duplication.
- **Asynchronous:** Always use `async/await` for I/O-bound operations.
- **Naming:** `PascalCase` for Classes/Methods, `camelCase` for variables, `I` prefix for interfaces.
- **Data Transfer:** Never expose entities directly; always use DTOs for API communication.
- **Error Handling:** Centralized via `ExceptionHandlingMiddleware` for a standard response format.
- **Validation:** All inputs must be validated with meaningful error messages.

## Core Modules & Flows
- **Authentication:** Registration, Login, and JWT management.
- **Car Listings:** Seller-driven flow (Create -> Detail -> Visibility).
- **Orders:** Buyer-driven flow (Select -> Confirm -> Order Created).
- **Admin:** Management of users and listings (including locking/unlocking).

---

## Specialized Agent Skills
When performing tasks in this repository, adhere to the following specialized "skills":

### 1. API Generator (`skills/api-generator.md`)
- **Role:** Senior .NET Backend Developer.
- **Goal:** Generate production-ready APIs following the `Controller → Service → Repository` pattern.
- **Requirements:** Use Clean Architecture, DTOs, input validation, and proper REST status codes.

### 2. Bug Fixer (`skills/bug-fixer.md`)
- **Role:** Senior .NET Debugging Specialist.
- **Goal:** Identify root causes and provide minimal, clean fixes.
- **Process:** Analyze → Identify → Suggest → Improve (without changing unrelated code).

## Supplemental Documentation
- `docs/project.md`: High-level project summary and business rules.
- `docs/flows.md`: Visualizing key application processes.
- `docs/architecture.md`: Deep dive into layer responsibilities.
- `docs/convensions.md`: Detailed coding and API standards.
