using OrderManagementSystem.Application.DTOs.OrderDetailViews;

namespace OrderManagementSystem.Application.Interfaces;

public interface IOrderDetailViewService
{
    Task<List<OrderDetailViewDto>> GetAllAsync();
    Task<OrderDetailViewDto?> GetByIdAsync(int orderId);
}
