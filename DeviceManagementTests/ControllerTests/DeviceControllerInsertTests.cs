using System.Net;
using System.Net.Http.Json;
using DeviceManagament.Domain.DTOs;
using DeviceManagament.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DeviceManagementTests.ControllerTests;

[TestFixture]
public class DeviceControllerInsertTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;

    [OneTimeSetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseUrls("http://localhost:5184");
            });
    
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("http://localhost:5184")
        });
    
        // Add default API version header
        _client.DefaultRequestHeaders.Add("api-version", "1.0");
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task InsertDevice_ValidDevice_ReturnsCreated()
    {
        // Arrange
        var deviceDto = new DeviceDto
        {
            ModelId = "MODEL123",
            ModelName = "Test Model",
            Manufacturer = "Test Manufacturer",
            PrimaryUser = "user@example.com",
            OperatingSystem = "Windows 11",
            DeviceType = nameof(DeviceType.Laptop),
            DeviceStatus = nameof(DeviceStatus.Active),
            SerialNumber = Guid.NewGuid().ToString()
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1.0/Device", deviceDto);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task InsertDevice_NullDevice_ReturnsBadRequest()
    {
        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/Device", (DeviceDto)null);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task InsertDevice_InvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var deviceDto = new DeviceDto
        {
            ModelId = "MODEL123",
            ModelName = "Test Model",
            Manufacturer = "Test Manufacturer",
            PrimaryUser = "invalid-email",
            OperatingSystem = "Windows 11",
            DeviceType = nameof(DeviceType.Laptop),
            DeviceStatus = nameof(DeviceStatus.Active),
            SerialNumber = Guid.NewGuid().ToString()
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/Device", deviceDto);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}
