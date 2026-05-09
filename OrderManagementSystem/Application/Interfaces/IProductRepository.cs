using OrderManagementSystem.Models;

namespace OrderManagementSystem.Application.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
}
