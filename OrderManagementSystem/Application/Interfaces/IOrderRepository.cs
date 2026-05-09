using OrderManagementSystem.Models;

namespace OrderManagementSystem.Application.Interfaces;

public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task CreateAsync(Order order);
    Task UpdateAsync(Order order);
}
