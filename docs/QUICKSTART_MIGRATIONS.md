# Quick Start: Database Migrations

**⚡ Fast guide to get your database up and running**

For comprehensive documentation, see [DATABASE_MIGRATIONS.md](DATABASE_MIGRATIONS.md)

---

## Prerequisites Check

✅ .NET 10.0 SDK installed  
✅ PostgreSQL server running  
✅ Connection string configured in `appsettings.local.json`

---

## 1️⃣ Install EF Core Tools (One-time Setup)

```bash
dotnet tool install --global dotnet-ef
```

Verify:
```bash
dotnet ef --version
```

---

## 2️⃣ Restore Packages

Navigate to project directory and restore packages (includes the newly added Design package):

```bash
cd DeviceManagament
dotnet restore
```

---

## 3️⃣ Configure Database Connection

Edit `appsettings.local.json` with your PostgreSQL credentials:

```json
{
  "ConnectionStrings": {
    "DeviceManagerDB": "Server=localhost;Database=DeviceManagementDB;User Id=your_user;Password=your_password;"
  }
}
```

---

## 4️⃣ Create Initial Migration

```bash
dotnet ef migrations add InitialCreate
```

This creates the migration files in a new `Migrations/` folder.

---

## 5️⃣ Apply Migration to Database

```bash
dotnet ef database update
```

This will:
- Create the `DeviceManagementDB` database (if it doesn't exist)
- Create the `devices` schema
- Create the `Devices` table with proper structure
- Add unique index on `SerialNumber`

---

## 6️⃣ Verify

Connect to PostgreSQL and verify:

```bash
psql -U your_user -d DeviceManagementDB
```

```sql
\dn                          -- List schemas (should see 'devices')
\dt devices.*                -- List tables in devices schema
\d devices."Devices"         -- Describe Devices table
```

---

## ✅ You're Done!

Your database is now ready. Run your application:

```bash
dotnet run
```

---

## Common Tasks

### Add a new column to Device model

1. Edit `Domain/Models/Device.cs`
2. Create migration:
   ```bash
   dotnet ef migrations add AddNewColumn
   ```
3. Apply migration:
   ```bash
   dotnet ef database update
   ```

### Rollback last migration

```bash
# List migrations to see names
dotnet ef migrations list

# Rollback to previous
dotnet ef database update <PreviousMigrationName>

# Remove the migration file
dotnet ef migrations remove
```

### Start fresh (⚠️ Deletes all data)

```bash
dotnet ef database drop --force
dotnet ef database update
```

---

## Need More Details?

See the comprehensive [DATABASE_MIGRATIONS.md](DATABASE_MIGRATIONS.md) guide for:
- Troubleshooting
- Best practices
- Environment-specific migrations
- Production deployment strategies
- Complete command reference

---

**Changed Files:**
- ✅ Added `Microsoft.EntityFrameworkCore.Design` package to `.csproj`
- ✅ Fixed connection string name in `Program.cs` (was `SampleApiDatabase`, now `DeviceManagerDB`)

**Created**: December 16, 2025

