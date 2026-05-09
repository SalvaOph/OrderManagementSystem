using OrderManagementSystem.Application.Dtos.Order;

namespace OrderManagementSystem.Application.Interfaces;

public interface IOrderService
{
    Task<List<OrderDto>> GetAllAsync();
    Task<OrderDto?> GetByIdAsync(int id);
    Task CreateAsync(CreateOrderDto dto);
    Task UpdateAsync(int id, UpdateOrderDto dto);
}
