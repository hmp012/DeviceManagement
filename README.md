# Device Management API

A robust RESTful API for managing IT devices built with ASP.NET Core 10.0, Entity Framework Core, and PostgreSQL. This API provides comprehensive device management capabilities including tracking device information, status, and user assignments.

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Latest-336791)](https://www.postgresql.org/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

## 🚀 Features

- **Device Management**: Create, update, and track IT devices (laptops, desktops)
- **RESTful API**: Clean, versioned API endpoints following REST principles
- **CQRS Pattern**: Implements Command Query Responsibility Segregation using MediatR
- **Repository Pattern**: Abstracted data access layer for maintainability
- **Exception Handling**: Global exception filter with proper HTTP status codes
- **API Versioning**: Built-in versioning support via URL segments and headers
- **Swagger/OpenAPI**: Interactive API documentation and testing
- **Docker Support**: Containerized PostgreSQL database setup
- **Entity Framework Core**: Code-first approach with migrations support

## 📋 Table of Contents

- [Architecture](#architecture)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Database Setup](#database-setup)
- [API Endpoints](#api-endpoints)
- [Project Structure](#project-structure)
- [Development](#development)
- [Testing](#testing)
- [Deployment](#deployment)
- [Contributing](#contributing)

## 🏗 Architecture

The application follows a clean architecture pattern with clear separation of concerns:

- **Controllers**: Handle HTTP requests and responses
- **Commands**: Implement CQRS command pattern using MediatR
- **Repositories**: Abstract data access operations
- **Domain Models**: Core business entities
- **DTOs**: Data transfer objects for API contracts
- **Exceptions**: Custom exception types with global handling
- **Database**: Entity Framework Core context and migrations

### Technologies Used

- **Framework**: ASP.NET Core 10.0
- **ORM**: Entity Framework Core 10.0.1
- **Database**: PostgreSQL (via Npgsql)
- **Mediator**: MediatR 14.0.0
- **API Versioning**: Asp.Versioning 8.1.0
- **Documentation**: Swashbuckle.AspNetCore 10.0.1
- **Containerization**: Docker & Docker Compose

## 📦 Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://www.docker.com/get-started) (for PostgreSQL)
- [PostgreSQL](https://www.postgresql.org/download/) (optional, if not using Docker)
- [Git](https://git-scm.com/downloads)
- A code editor ([JetBrains Rider](https://www.jetbrains.com/rider/), [Visual Studio](https://visualstudio.microsoft.com/), or [VS Code](https://code.visualstudio.com/))

### Optional Tools

- [dotnet-ef](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) - EF Core command-line tools
- [Postman](https://www.postman.com/) or [Insomnia](https://insomnia.rest/) - API testing

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd DeviceManagament
```

### 2. Start PostgreSQL Database

Using Docker Compose:

```bash
# Create a .env file with database credentials
cat > .env << EOF
POSTGRES_USER=devuser
POSTGRES_PASSWORD=devpassword
POSTGRES_DB=DeviceManagementDB
POSTGRES_PORT=5432
EOF

# Start the database
docker-compose up -d
```

### 3. Configure Connection String

Create `appsettings.local.json` in the `DeviceManagament` folder:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DeviceManagerDB": "Host=localhost;Database=DeviceManagementDB;Username=devuser;Password=devpassword;Port=5432"
  }
}
```

### 4. Install Dependencies

```bash
cd DeviceManagament
dotnet restore
```

### 5. Apply Database Migrations

```bash
# Install EF Core tools (if not already installed)
dotnet tool install --global dotnet-ef

# Apply migrations
dotnet ef database update
```

### 6. Run the Application

```bash
dotnet run
```

The API will be available at:
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`
- Swagger UI: `https://localhost:5001/swagger`

## ⚙️ Configuration

### Environment Variables

The application supports different configuration files based on the environment:

- `appsettings.json` - Base configuration
- `appsettings.Development.json` - Development environment
- `appsettings.local.json` - Local development (gitignored)

### Connection Strings

Configure your PostgreSQL connection in the appropriate `appsettings` file:

```json
{
  "ConnectionStrings": {
    "DeviceManagerDB": "Host=<host>;Database=<database>;Username=<username>;Password=<password>;Port=<port>"
  }
}
```

### Docker Environment Variables

For Docker deployment, configure these environment variables:

- `POSTGRES_USER` - Database user
- `POSTGRES_PASSWORD` - Database password
- `POSTGRES_DB` - Database name
- `POSTGRES_PORT` - Database port (default: 5432)

## 🗄️ Database Setup

### Entity Framework Core Migrations

#### Create a New Migration

```bash
dotnet ef migrations add <MigrationName> --project DeviceManagament
```

#### Apply Migrations

```bash
dotnet ef database update --project DeviceManagament
```

#### Remove Last Migration

```bash
dotnet ef migrations remove --project DeviceManagament
```

#### View Migration SQL

```bash
dotnet ef migrations script --project DeviceManagament
```

For detailed migration instructions, see [docs/DATABASE_MIGRATIONS.md](docs/DATABASE_MIGRATIONS.md).

### Database Schema

#### Device Table

| Column | Type | Description |
|--------|------|-------------|
| SerialNumber | GUID (PK) | Unique device identifier |
| ModelId | String | Device model identifier |
| ModelName | String | Device model name |
| Manufacturer | String | Device manufacturer |
| PrimaryUser | String (Email) | Email of primary user |
| OperatingSystem | String | Operating system name |
| DeviceType | Enum | Laptop or Desktop |
| DeviceStatus | Enum | Active, Inactive, or Retired |

## 📡 API Endpoints

### Base URL

```
https://localhost:5001/api/v1/Device
```

### Available Endpoints

#### Create Device

```http
POST /api/v1/Device
Content-Type: application/json

{
  "serialNumber": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "modelId": "XPS-15-9500",
  "modelName": "XPS 15",
  "manufacturer": "Dell",
  "primaryUser": "user@example.com",
  "operatingSystem": "Windows 11",
  "deviceType": "Laptop",
  "deviceStatus": "Active"
}
```

**Response**: `201 Created`

```json
{
  "serialNumber": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "modelId": "XPS-15-9500",
  "modelName": "XPS 15",
  "manufacturer": "Dell",
  "primaryUser": "user@example.com",
  "operatingSystem": "Windows 11",
  "deviceType": "Laptop",
  "deviceStatus": "Active"
}
```

**Error Responses**:
- `400 Bad Request` - Invalid device data
- `409 Conflict` - Device already exists
- `500 Internal Server Error` - Server error

#### Update Device

```http
PATCH /api/v1/Device/{serialNumber}
Content-Type: application/json

{
  "serialNumber": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "modelId": "XPS-15-9500",
  "modelName": "XPS 15",
  "manufacturer": "Dell",
  "primaryUser": "newuser@example.com",
  "operatingSystem": "Windows 11 Pro",
  "deviceType": "Laptop",
  "deviceStatus": "Inactive"
}
```

**Response**: `200 OK`

```json
{
  "serialNumber": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "modelId": "XPS-15-9500",
  "modelName": "XPS 15",
  "manufacturer": "Dell",
  "primaryUser": "newuser@example.com",
  "operatingSystem": "Windows 11 Pro",
  "deviceType": "Laptop",
  "deviceStatus": "Inactive"
}
```

**Error Responses**:
- `400 Bad Request` - Serial number mismatch or invalid data
- `404 Not Found` - Device not found
- `500 Internal Server Error` - Server error

### Device Types

- `Laptop`
- `Desktop`

### Device Statuses

- `Active`
- `Inactive`
- `Retired`

### API Versioning

The API supports versioning through:

1. **URL Segment** (Recommended):
   ```
   https://localhost:5001/api/v1/Device
   ```

2. **Header**:
   ```
   X-Api-Version: 1.0
   ```

## 📁 Project Structure

```
DeviceManagament/
├── DeviceManagament/               # Main API project
│   ├── Commands/                   # MediatR command handlers
│   │   ├── InsertDeviceCommand.cs
│   │   └── UpdateDeviceCommand.cs
│   ├── Controllers/                # API controllers
│   │   └── DeviceController.cs
│   ├── Database/                   # EF Core context
│   │   ├── DeviceManagerDbContext.cs
│   │   └── DeviceManagerDbContextFactory.cs
│   ├── Domain/                     # Domain layer
│   │   ├── DTOs/
│   │   │   └── DeviceDto.cs
│   │   └── Models/
│   │       └── Device.cs
│   ├── Exceptions/                 # Custom exceptions
│   │   ├── DeviceAlreadyExistsException.cs
│   │   ├── ExceptionFilter.cs
│   │   └── InvalidDeviceDataException.cs
│   ├── Migrations/                 # EF Core migrations
│   ├── Repositories/               # Data access layer
│   │   └── DeviceRepository.cs
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── DeviceManagament.csproj
│   ├── Dockerfile
│   └── Program.cs
├── DeviceManagementTests/          # Unit and integration tests
│   └── ControllerTests/
│       ├── DeviceControllerInsertTests.cs
│       └── DeviceControllerUpdateTests.cs
├── docs/                           # Documentation
│   ├── DATABASE_MIGRATIONS.md
│   └── QUICKSTART_MIGRATIONS.md
├── docker-compose.yml              # Docker configuration
└── DeviceManagament.sln           # Solution file
```

## 🛠️ Development

### Code Style

This project follows standard C# coding conventions:

- Use PascalCase for public members
- Use camelCase for private fields
- Use meaningful variable names
- Keep methods focused and small
- Write XML documentation for public APIs

### Adding New Features

1. **Create Domain Models** in `Domain/Models/`
2. **Create DTOs** in `Domain/DTOs/`
3. **Create Commands** in `Commands/` using MediatR
4. **Create Repository Interface & Implementation** in `Repositories/`
5. **Create Controller Endpoints** in `Controllers/`
6. **Add Migrations** using `dotnet ef migrations add`
7. **Write Tests** in `DeviceManagementTests/`

### Running in Development Mode

```bash
# Set environment to Development
export ASPNETCORE_ENVIRONMENT=Development  # Linux/macOS
$env:ASPNETCORE_ENVIRONMENT="Development"  # Windows PowerShell

# Run with hot reload
dotnet watch run
```

### Building the Project

```bash
# Debug build
dotnet build

# Release build
dotnet build -c Release

# Publish
dotnet publish -c Release -o ./publish
```

## 🧪 Testing

### Running Tests

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --verbosity detailed

# Run with code coverage
dotnet test /p:CollectCoverage=true
```

### Test Structure

Tests are organized in the `DeviceManagementTests` project:

```
DeviceManagementTests/
└── ControllerTests/
    ├── DeviceControllerInsertTests.cs
    └── DeviceControllerUpdateTests.cs
```

### Writing Tests

Example test structure:

```csharp
[Fact]
public async Task InsertDevice_ValidDevice_ReturnsCreated()
{
    // Arrange
    var deviceDto = new DeviceDto { /* ... */ };
    
    // Act
    var result = await _controller.InsertDevice(deviceDto);
    
    // Assert
    Assert.IsType<CreatedResult>(result);
}
```

## 🐳 Docker Deployment

### Using Docker Compose

```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f

# Stop services
docker-compose down

# Remove volumes
docker-compose down -v
```

### Building Docker Image

```bash
# Build the API image
docker build -t device-management-api:latest -f DeviceManagament/Dockerfile .

# Run the container
docker run -p 5000:8080 -e ConnectionStrings__DeviceManagerDB="<connection-string>" device-management-api:latest
```

## 📊 API Documentation

When running in Development or Local environment, Swagger UI is available at:

```
https://localhost:5001/swagger
```

Swagger provides:
- Interactive API documentation
- Request/response schemas
- Try-it-out functionality
- API versioning support

## 🔧 Troubleshooting

### Common Issues

#### 1. Database Connection Failed

**Problem**: Cannot connect to PostgreSQL database

**Solution**:
- Verify PostgreSQL is running: `docker-compose ps`
- Check connection string in `appsettings.local.json`
- Ensure database exists: `docker exec -it devicemanagement-db psql -U devuser -d DeviceManagementDB`

#### 2. Migration Errors

**Problem**: EF Core migration fails

**Solution**:
```bash
# Remove last migration
dotnet ef migrations remove

# Clear database and reapply
dotnet ef database drop
dotnet ef database update
```

#### 3. Port Already in Use

**Problem**: Port 5000/5001 already occupied

**Solution**:
- Change ports in `Properties/launchSettings.json`
- Or kill the process using the port:
  ```bash
  # macOS/Linux
  lsof -ti:5000 | xargs kill -9
  
  # Windows
  netstat -ano | findstr :5000
  taskkill /PID <PID> /F
  ```

#### 4. Swagger Not Loading

**Problem**: Swagger UI not accessible

**Solution**:
- Ensure environment is Development or Local
- Check `Program.cs` Swagger configuration
- Verify running on correct URL

### Debug Logging

Enable detailed logging in `appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Debug",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

## 📝 Additional Documentation

- [Database Migrations Guide](docs/DATABASE_MIGRATIONS.md) - Comprehensive EF Core migrations documentation
- [Quick Start Migrations](docs/QUICKSTART_MIGRATIONS.md) - Fast migration setup guide

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Commit your changes: `git commit -m 'Add amazing feature'`
4. Push to the branch: `git push origin feature/amazing-feature`
5. Open a Pull Request

### Contribution Guidelines

- Follow existing code style and conventions
- Write unit tests for new features
- Update documentation as needed
- Ensure all tests pass before submitting PR
- Keep commits atomic and well-described

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Authors

- Your Name - Initial work

## 🙏 Acknowledgments

- ASP.NET Core team for the excellent framework
- Entity Framework Core team
- MediatR library contributors
- PostgreSQL community

## 📞 Support

For issues and questions:
- Create an issue in the repository
- Check existing documentation in `/docs`
- Review troubleshooting section above

## 🗺️ Roadmap

Future enhancements:

- [ ] Add GET endpoints for retrieving devices
- [ ] Implement pagination for device lists
- [ ] Add filtering and search capabilities
- [ ] Implement authentication and authorization
- [ ] Add device history tracking
- [ ] Create admin dashboard
- [ ] Add bulk operations support
- [ ] Implement caching layer
- [ ] Add health check endpoints
- [ ] Implement rate limiting

---

**Built with ❤️ using ASP.NET Core**

