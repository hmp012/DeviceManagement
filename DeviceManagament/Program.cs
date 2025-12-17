using Asp.Versioning;
using DeviceManagament.Commands;
using DeviceManagament.Database;
using DeviceManagament.Exceptions;
using DeviceManagament.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.
builder.Services.AddControllers();

// ---> ADD THESE TWO LINES <---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version")
    );
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

string connectionString = builder.Configuration.GetConnectionString("DeviceManagerDB") ??
                          throw new InvalidOperationException("DeviceManagerDB connection string is required");

builder.Services.AddDbContext<DeviceManagerDbContext>(opt => 
{
    opt.UseNpgsql(connectionString);
});

builder.Services.AddScoped<ExceptionFilter>();

builder.Services.AddScoped<DeviceManagerDbContext>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

var app = builder.Build();

// 2. Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Local"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();