# Database Migrations Guide

## Overview

This guide provides comprehensive instructions for managing Entity Framework Core migrations in the DeviceManagement project. The project uses PostgreSQL as the database and EF Core 10.0.1 for data access.

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Database Configuration](#database-configuration)
3. [Installing EF Core Tools](#installing-ef-core-tools)
4. [Creating Migrations](#creating-migrations)
5. [Applying Migrations](#applying-migrations)
6. [Common Migration Commands](#common-migration-commands)
7. [Migration Best Practices](#migration-best-practices)
8. [Troubleshooting](#troubleshooting)
9. [Environment-Specific Migrations](#environment-specific-migrations)

---

## Prerequisites

Before working with migrations, ensure you have:

- **.NET 10.0 SDK** installed
- **PostgreSQL** database server running
- **EF Core Tools** installed (see [Installing EF Core Tools](#installing-ef-core-tools))
- Proper **connection string** configured in `appsettings.json` or `appsettings.local.json`

---

## Database Configuration

### Current Database Setup

- **Database Provider**: PostgreSQL (Npgsql)
- **Schema**: `devices` (default schema)
- **DbContext**: `DeviceManagerDbContext`

### Connection String Configuration

The project uses different configuration files for different environments:

1. **appsettings.json** - Base configuration (placeholder)
2. **appsettings.local.json** - Local development configuration
3. **appsettings.Development.json** - Development environment configuration

**Current connection string location**: `ConnectionStrings:DeviceManagerDB`

**Note**: There's a mismatch in the Program.cs file - it references `SampleApiDatabase` but the appsettings files use `DeviceManagerDB`. You may need to update this.

Example connection string format:
```json
{
  "ConnectionStrings": {
    "DeviceManagerDB": "Server=localhost;Database=DeviceManagementDB;User Id=dev_user;Password=dev_password;"
  }
}
```

---

## Installing EF Core Tools

### Option 1: Global Tool (Recommended)

Install the EF Core global tool to use `dotnet ef` commands:

```bash
dotnet tool install --global dotnet-ef
```

To update an existing installation:

```bash
dotnet tool update --global dotnet-ef
```

Verify installation:

```bash
dotnet ef --version
```

### Option 2: Local Tool (Project-specific)

Create a tool manifest if it doesn't exist:

```bash
dotnet new tool-manifest
```

Install EF Core tools locally:

```bash
dotnet tool install dotnet-ef
```

When using local tools, prefix commands with `dotnet tool run`:

```bash
dotnet tool run dotnet-ef --version
```

---

## Creating Migrations

### Basic Migration Creation

Navigate to the project directory containing the `.csproj` file:

```bash
cd /Users/dkhumape/Documents/DeviceManagement/DeviceManagament/DeviceManagament
```

Create a new migration:

```bash
dotnet ef migrations add <MigrationName>
```

**Naming Convention**: Use descriptive PascalCase names that describe the changes:

- `InitialCreate` - First migration creating all tables
- `AddDeviceSerialNumberIndex` - Adding an index
- `UpdateDeviceStatusEnum` - Modifying enum values
- `AddDeviceLastUpdatedColumn` - Adding new column

### Example: Creating Initial Migration

```bash
dotnet ef migrations add InitialCreate
```

This will:
1. Analyze your `DeviceManagerDbContext` and entity models
2. Create a new migration file in the `Migrations/` folder
3. Generate `<timestamp>_InitialCreate.cs` and `<timestamp>_InitialCreate.Designer.cs`
4. Update `DeviceManagerDbContextModelSnapshot.cs`

### Specifying Project and Startup Project

If running from solution directory:

```bash
dotnet ef migrations add <MigrationName> \
  --project DeviceManagament/DeviceManagament.csproj \
  --startup-project DeviceManagament/DeviceManagament.csproj
```

### Specifying Output Directory

```bash
dotnet ef migrations add <MigrationName> --output-dir Database/Migrations
```

---

## Applying Migrations

### Apply to Database

Apply all pending migrations to the database:

```bash
dotnet ef database update
```

### Apply Specific Migration

Update to a specific migration:

```bash
dotnet ef database update <MigrationName>
```

### Rollback to Previous Migration

Rollback by specifying an earlier migration:

```bash
dotnet ef database update <PreviousMigrationName>
```

Rollback all migrations (removes all tables):

```bash
dotnet ef database update 0
```

### Apply at Application Startup (Alternative Approach)

You can configure automatic migration application in `Program.cs`:

```csharp
var app = builder.Build();

// Apply migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DeviceManagerDbContext>();
    dbContext.Database.Migrate();
}

// Continue with app configuration...
```

**⚠️ Warning**: Automatic migrations are generally **not recommended for production** environments. Use explicit migration commands instead.

---

## Common Migration Commands

### List All Migrations

View all migrations and their status:

```bash
dotnet ef migrations list
```

### View Migration SQL

Generate SQL script without applying:

```bash
dotnet ef migrations script
```

Generate SQL for specific migration range:

```bash
dotnet ef migrations script <FromMigration> <ToMigration>
```

Generate SQL for all migrations:

```bash
dotnet ef migrations script --output migration.sql
```

Generate idempotent SQL (can be run multiple times safely):

```bash
dotnet ef migrations script --idempotent
```

### Remove Last Migration

Remove the most recent migration (if not yet applied):

```bash
dotnet ef migrations remove
```

**⚠️ Warning**: Only works if the migration hasn't been applied to the database.

If migration is already applied, you must:
1. First rollback: `dotnet ef database update <PreviousMigration>`
2. Then remove: `dotnet ef migrations remove`

### Drop Database

**⚠️ DANGER**: This completely drops the database:

```bash
dotnet ef database drop
```

With force flag (no confirmation):

```bash
dotnet ef database drop --force
```

---

## Migration Best Practices

### 1. Version Control

✅ **DO**:
- Commit migration files to version control
- Include all three files: `*.cs`, `*.Designer.cs`, and `*ModelSnapshot.cs`
- Review migration code before committing

❌ **DON'T**:
- Modify migration files manually after creation (use new migrations instead)
- Delete old migrations that have been applied to production

### 2. Team Collaboration

When working in a team:

1. **Pull latest changes** before creating migrations
2. **Resolve conflicts** in migrations carefully
3. **Communicate** with team about database changes
4. **Test migrations** in development before applying to production

### 3. Migration Naming

Use descriptive, action-based names:
- ✅ `AddDeviceLastModifiedColumn`
- ✅ `CreateDeviceAuditTable`
- ✅ `UpdateDeviceSerialNumberToGuid`
- ❌ `Migration1`, `UpdateDevice`, `Fix`

### 4. Data Migrations

For data migrations, use the `migrationBuilder.Sql()` method:

```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.Sql(
        "UPDATE devices.\"Devices\" SET \"DeviceStatus\" = 0 WHERE \"DeviceStatus\" IS NULL"
    );
}
```

### 5. Testing Migrations

Before applying to production:

1. Test on local database
2. Test on staging/test environment
3. Generate SQL script and review
4. Plan rollback strategy
5. Backup production database

### 6. Production Deployments

For production environments:

1. **Generate SQL script**:
   ```bash
   dotnet ef migrations script --idempotent --output deploy.sql
   ```

2. **Review the script** thoroughly

3. **Backup database** before applying

4. **Apply during maintenance window** if possible

5. **Test application** after migration

---

## Troubleshooting

### Issue: Connection String Not Found

**Error**: `InvalidOperationException: SampleApiDatabase connection string is required`

**Solution**: 
- Check that `appsettings.json` or `appsettings.local.json` contains the connection string
- Note: Your Program.cs references `SampleApiDatabase` but config files use `DeviceManagerDB`
- Update Program.cs line 29 to use `DeviceManagerDB` or rename the config entry

```csharp
// Update this line in Program.cs
string connectionString = builder.Configuration.GetConnectionString("DeviceManagerDB") ??
                          throw new InvalidOperationException("DeviceManagerDB connection string is required");
```

### Issue: PostgreSQL Connection Failed

**Error**: `Npgsql.NpgsqlException: Connection refused`

**Solutions**:
1. Verify PostgreSQL is running: `pg_isready`
2. Check connection string parameters (host, port, database, credentials)
3. Verify PostgreSQL is listening on the correct port (default: 5432)
4. Check firewall settings

### Issue: Schema Not Found

**Error**: `Npgsql.PostgresException: schema "devices" does not exist`

**Solution**: The first migration will create the schema. Ensure you've applied migrations:
```bash
dotnet ef database update
```

### Issue: Migration Already Applied

**Error**: Trying to remove a migration that's been applied

**Solution**:
```bash
# Rollback first
dotnet ef database update <PreviousMigrationName>
# Then remove
dotnet ef migrations remove
```

### Issue: Build Errors

**Error**: `Build failed`

**Solution**:
1. Fix compilation errors in your code first
2. Clean and rebuild:
   ```bash
   dotnet clean
   dotnet build
   ```
3. Try migration command again

### Issue: Multiple DbContext Classes

**Error**: `More than one DbContext was found`

**Solution**: Specify the context explicitly:
```bash
dotnet ef migrations add <MigrationName> --context DeviceManagerDbContext
```

---

## Environment-Specific Migrations

### Development Environment

```bash
# Use local connection string
export ASPNETCORE_ENVIRONMENT=Development
dotnet ef database update
```

### Using Different Connection Strings

#### Option 1: Environment Variables

Set connection string via environment variable:

```bash
export ConnectionStrings__DeviceManagerDB="Server=localhost;Database=DeviceManagementDB;User Id=dev_user;Password=dev_password;"
dotnet ef database update
```

#### Option 2: Command Line

```bash
dotnet ef database update --connection "Server=localhost;Database=DeviceManagementDB;User Id=dev_user;Password=dev_password;"
```

#### Option 3: Different Config File

Create `appsettings.Production.json`:

```json
{
  "ConnectionStrings": {
    "DeviceManagerDB": "Server=prod-server;Database=DeviceManagementDB;User Id=prod_user;Password=prod_password;"
  }
}
```

Run with production environment:

```bash
export ASPNETCORE_ENVIRONMENT=Production
dotnet ef database update
```

---

## Quick Reference

### Complete Workflow: First Migration

```bash
# 1. Navigate to project directory
cd /Users/dkhumape/Documents/DeviceManagement/DeviceManagament/DeviceManagament

# 2. Ensure EF Core tools are installed
dotnet tool install --global dotnet-ef

# 3. Create initial migration
dotnet ef migrations add InitialCreate

# 4. Review generated migration files in Migrations/ folder

# 5. Apply migration to database
dotnet ef database update

# 6. Verify tables were created
# (Use your preferred PostgreSQL client or psql)
```

### Workflow: Adding New Changes

```bash
# 1. Modify your entity models or DbContext

# 2. Create migration for the changes
dotnet ef migrations add <DescriptiveName>

# 3. Review the generated migration

# 4. Apply to database
dotnet ef database update

# 5. Test your application
```

### Workflow: Rolling Back Changes

```bash
# 1. List migrations
dotnet ef migrations list

# 2. Rollback to previous migration
dotnet ef database update <PreviousMigrationName>

# 3. Remove the bad migration
dotnet ef migrations remove

# 4. Make corrections and create new migration
```

---

## Current Project Entities

### Device Entity

Located in: `Domain/Models/Device.cs`

**Properties**:
- `SerialNumber` (Guid) - Primary Key
- `ModelId` (string)
- `ModelName` (string)
- `Manufacturer` (string)
- `PrimaryUser` (string) - Email format
- `OperatingSystem` (string)
- `DeviceType` (enum) - Laptop, Desktop
- `DeviceStatus` (enum) - Active, Inactive, Retired

**Indexes**:
- Unique index on `SerialNumber`

**Schema**: `devices`

---

## Additional Resources

- [EF Core Migrations Documentation](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [PostgreSQL EF Core Provider](https://www.npgsql.org/efcore/)
- [EF Core CLI Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

---

## Notes

1. The connection string name in `Program.cs` (`SampleApiDatabase`) doesn't match the name in config files (`DeviceManagerDB`). Consider standardizing this.

2. Currently, the project doesn't have a `Migrations` folder, indicating no migrations have been created yet.

3. The project uses .NET 10.0 and EF Core 10.0.1, which are the latest versions.

4. Make sure to add the `Microsoft.EntityFrameworkCore.Design` package if you encounter issues:
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore.Design --version 10.0.1
   ```

---

**Last Updated**: December 16, 2025  
**Project**: DeviceManagement  
**Database**: PostgreSQL  
**EF Core Version**: 10.0.1

