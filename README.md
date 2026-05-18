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

Technology Stack:

C#
ASP.NET MVC 5
ASP.NET Web API
.NET Framework 4.5.2
Entity Framework 6.2
Entity Framework Core 1.1 packages
Razor Views
SQL Server
JavaScript
CSS
HTML
Newtonsoft.Json
NuGet Package Restore
Main Modules

The application includes the following main areas:

Login and authentication
Dashboard
User management
Role management
Permission management
Product management
Access control
Logging
Utility/common services
Service/API layer
Architecture

The solution follows a layered structure:

1. COD - Presentation Layer

The COD project is the main ASP.NET MVC web application. It contains controllers, views, models, configuration files, and frontend assets.

Important folders:

Controllers
Models
Views
Assets
App_Start
ConfigFiles

Main controllers include:

LoginController
DashboardController
UserController
RoleController
PermissionController
ProductController
BaseController
ErrorController
2. BAL - Business Layer

The BAL project contains repository classes and business logic. It acts as the middle layer between the web application and the data access layer.

Important repositories include:

BaseRepository
userRepository
roleRepository
permissionRepository
productRepository
accessControlRepository
logRepository
utilityRepository
appClientRepository
3. DAL - Data Access Layer

The DAL project contains Entity Framework database entities and data models. It is responsible for database communication and persistence.

Important folders:

DBEntities
Models
Resources
4. ServiceCOD - Service Layer

The ServiceCOD project appears to provide service/API functionality using ASP.NET MVC/Web API dependencies.

Features
Layered architecture
ASP.NET MVC-based web interface
Entity Framework database integration
User authentication
Role-based access management
Permission management
Product management
Dashboard module
Repository-based business logic
Separate data access project
Service/API project
Razor-based UI
NuGet package restore support
Requirements

To run this project, you need:

Visual Studio 2019 or later
.NET Framework 4.5.2 Developer Pack
SQL Server
NuGet Package Restore enabled
IIS Express or IIS
