using Microsoft.AspNetCore.Mvc;

namespace OrderManagementSystem.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly Application.Interfaces.ICustomerService _service;

    public CustomerController(Application.Interfaces.ICustomerService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await _service.GetAllAsync();
        return Ok(customers);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerById(string id)
    {
        var customer = await _service.GetByIdAsync(id);
        if (customer == null) return NotFound();
        return Ok(customer);
    }
}