
# Bonta E-Commerce API

This is the backend API for the Bonta e-commerce platform. It is built using a clean, scalable architecture with .NET and ASP.NET Core, designed for security, performance, and maintainability. The system provides a complete set of features for managing products, users, and payments.

## Core Technologies & Architecture

This project follows modern .NET best practices and utilizes a robust technology stack:

*   **Framework:** .NET 9 / ASP.NET Core Web API
*   **Architecture:** Clean/Onion Architecture (implied by `Core`, `Infrastructure`, `Web` separation)
*   **Database:** PostgreSQL
*   **Data Access:** Entity Framework Core 9
*   **Authentication:** JWT Bearer Tokens via ASP.NET Core Identity
*   **Caching:** Redis for in-memory caching
*   **Payments:** Stripe Integration
*   **Object Mapping:** AutoMapper
*   **Deployment:** Designed for containerization and PaaS platforms like Render or Azure.

## Features

*   **RESTful API:** A well-structured API for all e-commerce operations.
*   **Product Catalog Management:** Full CRUD operations for Products, Brands, and Types.
*   **Secure JWT Authentication:** Role-based access control using `Admin` and `SuperAdmin` roles.
*   **Stripe Payment Integration:** Secure handling of payment secrets and webhook endpoints.
*   **Secure Image Handling:** Endpoints for uploading product images with validation to prevent malicious file uploads. Image URLs are dynamically resolved with a base URL.
*   **Database Seeding:** A `DbInitializer` populates the database with initial data from JSON files on startup, making setup easy.
*   **Dual DbContexts:**
    *   `StoreDbContext`: Manages the main application data (products, orders, etc.).
    *   `StoreIdentityDbContext`: Manages users, roles, and claims via ASP.NET Core Identity.
*   **Centralized Exception Handling:** Custom middleware to provide consistent error responses.

---

## Getting Started

Follow these instructions to get the project up and running on your local machine for development and testing purposes.

### Prerequisites

You will need the following software installed on your machine:

*   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
*   [PostgreSQL](https://www.postgresql.org/download/) with pgAdmin (recommended)
*   [Redis](https://redis.io/docs/getting-started/installation/) (can be run locally or via Docker)
*   A code editor like [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### 1. Clone the Repository

```bash
git clone <your-repository-url>
cd <your-project-directory>
```

### 2. Configure Local Secrets

This project uses the **.NET User Secrets** tool to manage sensitive data locally. It will **not** run without these settings.

Navigate to the startup project directory (the one containing `Program.cs`, e.g., `E-Commerce.Web`) and run the following commands:

```bash
# Initialize user secrets for the project
dotnet user-secrets init

# Set your JWT and Stripe secrets
dotnet user-secrets set "JWTOptions:SecretKey" "YOUR_SUPER_LONG_AND_SECRET_JWT_KEY_HERE"
dotnet user-secrets set "Stripe:SecretKey" "sk_test_..."
dotnet user-secrets set "Stripe:EndPointSecret" "whsec_..."
```

### 3. Configure Development Settings

In your startup project, ensure you have an `appsettings.Development.json` file. This file overrides `appsettings.json` for your local environment and **should be in your `.gitignore`**.

**`appsettings.Development.json`:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=5432;Database=MyStoreDb;User Id=postgres;Password=YOUR_POSTGRES_PASSWORD",
    "IdentityConnection": "Server=localhost;Port=5432;Database=MyIdentityDb;User Id=postgres;Password=YOUR_POSTGRES_PASSWORD"
  },
  "JWTOptions": {
    "Issuer": "https://localhost:7003",
    "Audience": "http://localhost:3000"
  },
  "BaseUrl": "https://localhost:7003/"
}
```

### 4. Setup the Databases

This project uses EF Core Migrations to manage the database schema.

**Using Package Manager Console (in Visual Studio):**
Make sure your API project is set as the Startup Project.

```powershell
# Create and apply migrations for the main application database
Update-Database -Context StoreDbContext

# Create and apply migrations for the identity database
Update-Database -Context StoreIdentityDbContext
```

**Using .NET CLI:**
Navigate to the root folder of your solution.

```bash
# Create and apply migrations for the main application database
dotnet ef database update --context StoreDbContext

# Create and apply migrations for the identity database
dotnet ef database update --context StoreIdentityDbContext
```

### 5. Run the Application

You can now run the project from Visual Studio (by pressing the "Play" button) or using the .NET CLI:

```bash
# Navigate to the startup project directory
dotnet run
```

The API will be available at `https://localhost:7003`. You can explore all available endpoints using the Swagger UI at `https://localhost:7003/swagger`.

---

## Deployment

The application is designed for deployment to a PaaS provider like **Render** or **Azure**. All configuration is handled via **Environment Variables**.

The table below shows the required environment variables and how they map from `appsettings.json`.

| Environment Variable Name              | Purpose                                       | Example Value (for Render)                            |
| -------------------------------------- | --------------------------------------------- | ----------------------------------------------------- |
| `ASPNETCORE_ENVIRONMENT`               | Sets the runtime environment                  | `Production`                                          |
| `ConnectionStrings__DefaultConnection` | Connection string for the main database       | `postgresql://user:pass@host/db` (from provider)      |
| `ConnectionStrings__IdentityConnection`| Connection string for the identity database   | `postgresql://user:pass@host/db` (from provider)      |
| `ConnectionStrings__Redis`             | Connection string for Redis cache             | `redis://user:pass@host:port` (from provider)       |
| `BaseUrl`                              | Public base URL for constructing image URLs   | `https://your-api-name.onrender.com/`                 |
| `JWTOptions__Issuer`                   | The public URL of the API                     | `https://your-api-name.onrender.com/`                 |
| `JWTOptions__Audience`                 | The public URL of your frontend application   | `https://your-frontend-app.com`                       |
| `JWTOptions__SecretKey`                | **Secret** for signing JWTs                   | `YOUR_SUPER_LONG_AND_SECRET_JWT_KEY_HERE`             |
| `Stripe__SecretKey`                    | **Secret** key for Stripe API                 | `sk_live_...` or `sk_test_...`                          |
| `Stripe__EndPointSecret`               | **Secret** for verifying Stripe webhooks      | `whsec_...`                                           |

---
