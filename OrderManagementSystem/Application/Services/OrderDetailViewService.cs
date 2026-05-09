using OrderManagementSystem.Application.DTOs.OrderDetailViews;
using OrderManagementSystem.Application.Interfaces;

namespace OrderManagementSystem.Application.Services;

public class OrderDetailViewService : IOrderDetailViewService
{
    private readonly IOrderDetailViewRepository _repository;
    public OrderDetailViewService(IOrderDetailViewRepository repository) => _repository = repository;

    public async Task<List<OrderDetailViewDto>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<OrderDetailViewDto?> GetByIdAsync(int orderId)
    {
        return await _repository.GetByIdAsync(orderId);
    }
}
