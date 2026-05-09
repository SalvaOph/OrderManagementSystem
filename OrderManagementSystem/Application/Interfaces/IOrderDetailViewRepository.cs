using OrderManagementSystem.Application.DTOs.OrderDetailViews;

namespace OrderManagementSystem.Application.Interfaces;

public interface IOrderDetailViewRepository
{
    Task<List<OrderDetailViewDto>> GetAllAsync();
    Task<OrderDetailViewDto?> GetByIdAsync(int orderId);
}
