using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Application.Dtos.OrderDetail;
using OrderManagementSystem.Application.Interfaces;

namespace OrderManagementSystem.Controllers;

[ApiController]
[Route("api/orderdetails")]
public class OrderDetailController : ControllerBase
{
    private readonly IOrderDetailService _service;

    public OrderDetailController(IOrderDetailService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await _service.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("{orderId}/{productId}")]
    public async Task<IActionResult> GetById(int orderId, int productId)
    {
        var dto = await _service.GetByIdAsync(orderId, productId);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateOrderDetailDto dto)
    {
        await _service.CreateAsync(dto);
        return Ok(dto);
    }

    [HttpPut("{orderId}/{productId}")]
    public async Task<IActionResult> Update(int orderId, int productId, [FromBody] UpdateOrderDetailDto dto)
    {
        var existing = await _service.GetByIdAsync(orderId, productId);
        if (existing == null) return NotFound();

        await _service.UpdateAsync(orderId, productId, dto);
        return NoContent();
    }
}
