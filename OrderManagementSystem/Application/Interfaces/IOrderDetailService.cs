using OrderManagementSystem.Application.Dtos.OrderDetail;

namespace OrderManagementSystem.Application.Interfaces;

public interface IOrderDetailService
{
    Task<List<OrderDetailDto>> GetAllAsync();
    Task<OrderDetailDto?> GetByIdAsync(int orderId, int productId);
    Task CreateAsync(CreateOrderDetailDto dto);
    Task UpdateAsync(int orderId, int productId, UpdateOrderDetailDto dto);
}
