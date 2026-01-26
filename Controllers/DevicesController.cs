using Microsoft.AspNetCore.Mvc;
using SimpleDeviceAPI.Contracts;
using SimpleDeviceAPI.Models;
using SimpleDeviceAPI.Services;

namespace SimpleDeviceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DevicesController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DeviceResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var devices = await _deviceService.GetAllDevicesAsync(ct);

        var response = devices.Select(d => new DeviceResponse
        {
            Id = d.Id,
            Name = d.Name,
            Status = d.Status
        });

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken ct)
    {
        var device = await _deviceService.GetByIdAsync(id, ct);
        if (device is null)
            return NotFound();

        var response = new DeviceResponse
        {
            Id = device.Id,
            Name = device.Name,
            Status = device.Status
        };

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateDeviceRequest request, CancellationToken ct)
    {
        if (request is null)
            return BadRequest();

        var entity = new Device
        {
            Name = request.Name.Trim(),
            Status = string.IsNullOrWhiteSpace(request.Status) ? "Unknown" : request.Status.Trim()
        };

        await _deviceService.AddDeviceAsync(entity, ct);

        var response = new DeviceResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            Status = entity.Status
        };

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }
}
