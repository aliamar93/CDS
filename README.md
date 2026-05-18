# CDS

CDS is an ASP.NET MVC 5 web application built with a layered architecture using C#, .NET Framework 4.5.2, Entity Framework, and SQL Server.

The solution is organized into separate projects for presentation, business logic, data access, and service/API functionality. It includes modules for users, roles, permissions, products, authentication, dashboard views, and access control.

## Repository

GitHub: https://github.com/aliamar93/CDS

## Project Structure

```text
CDS/
│
├── COD/              # Main ASP.NET MVC web application
│   ├── Controllers/  # MVC controllers
│   ├── Models/       # View models, enums, settings, common services
│   ├── Views/        # Razor views
│   ├── Assets/       # CSS, JavaScript, images, frontend assets
│   └── Web.config    # Web application configuration
│
├── BAL/              # Business Access Layer / Repository logic
│   └── Repositories/ # Business repositories and shared logic
│
├── DAL/              # Data Access Layer
│   ├── DBEntities/   # Entity Framework database model
│   ├── Models/       # Data models
│   └── Resources/    # Resource files
│
├── ServiceCOD/       # Service/API project
│
├── packages/         # NuGet packages
│
└── COD.sln           # Visual Studio solution file
