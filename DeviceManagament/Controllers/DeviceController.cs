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
    public Task<IActionResult> InsertDevice([FromBody] DeviceDto device)
    {
        // Implementation for inserting a device
        throw new NotImplementedException();
    }

    [HttpPatch("{serialNumber}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Device), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
    public Task<IActionResult> UpdateDevice([FromRoute] Guid serialNumber, [FromBody] DeviceDto device)
    {
        // Implementation for updating a device
        throw new NotImplementedException();
    }
}