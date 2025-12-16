using System.Net;
using System.Net.Http.Json;
using DeviceManagament.Domain.DTOs;
using DeviceManagament.Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DeviceManagementTests.ControllerTests;

[TestFixture]
public class DeviceControllerUpdateTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [Test]
    public async Task UpdateDevice_ValidSerialNumberAndDevice_ReturnsOk()
    {
        // Arrange
        var serialNumber = Guid.NewGuid();
        var deviceDto = new DeviceDto
        {
            ModelId = "MODEL123",
            ModelName = "Updated Model",
            Manufacturer = "Test Manufacturer",
            PrimaryUser = "user@example.com",
            OperatingSystem = "Windows 11",
            DeviceType = nameof(DeviceType.Desktop),
            DeviceStatus = nameof(DeviceStatus.Active),
            SerialNumber = Guid.NewGuid().ToString()
        };

        // Act
        var response = await _client.PatchAsJsonAsync($"/api/v1/Device/{serialNumber}", deviceDto);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task UpdateDevice_NonExistentSerialNumber_ReturnsNotFound()
    {
        // Arrange
        var serialNumber = Guid.NewGuid();
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
        var response = await _client.PatchAsJsonAsync($"/api/v1/Device/{serialNumber}", deviceDto);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task UpdateDevice_NullDevice_ReturnsBadRequest()
    {
        // Arrange
        var serialNumber = Guid.NewGuid();

        // Act
        var response = await _client.PatchAsJsonAsync($"/api/v1/Device/{serialNumber}", (DeviceDto)null);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task UpdateDevice_EmptyGuid_ReturnsBadRequest()
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
        var response = await _client.PatchAsJsonAsync($"/api/v1/Device/{Guid.Empty}", deviceDto);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}
