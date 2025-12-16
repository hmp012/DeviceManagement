using Asp.Versioning;
using DeviceManagament.Domain.DTOs;
using DeviceManagament.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagament.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class DeviceController: ControllerBase
{
    
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Device), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(InternalServerError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> InsertDevice([FromBody] DeviceDto device)
    {
        // Implementation for inserting a device
        return Created(nameof(InsertDevice), device);
    }

    [HttpPatch("{serialNumber}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Device), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDevice([FromRoute] Guid serialNumber, [FromBody] DeviceDto device)
    {
        // Implementation for updating a device
        return Ok(device);
    }
}