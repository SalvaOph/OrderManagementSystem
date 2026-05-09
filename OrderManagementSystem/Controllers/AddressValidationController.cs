using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Application.DTOs.AdressValidation;
using OrderManagementSystem.Application.Interfaces;

namespace OrderManagementSystem.Controllers;

[ApiController]
[Route("api/address")]
public class AddressController : ControllerBase
{
    private readonly IGoogleMapsService _service;

    public AddressController(IGoogleMapsService service)
    {
        _service = service;
    }

    [HttpPost("validate")]
    public async Task<IActionResult> Validate([FromBody] AddressValidationRequest request)
    {
        var result = await _service.ValidateAddress(request.Address);

        if (result == null)
            return BadRequest("Invalid address");

        return Ok(result);
    }
}