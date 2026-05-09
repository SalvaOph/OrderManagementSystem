using OrderManagementSystem.Models;

namespace OrderManagementSystem.Application.Interfaces;

public interface IOrderDetailRepository
{
    Task<List<OrderDetail>> GetAllAsync();
    Task<OrderDetail?> GetByIdAsync(int orderId, int productId);
    Task CreateAsync(OrderDetail orderDetail);
    Task UpdateAsync(OrderDetail orderDetail);
}
