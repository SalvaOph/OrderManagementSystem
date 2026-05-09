using OrderManagementSystem.Application.Dtos.Product;

namespace OrderManagementSystem.Application.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(int id);
}
