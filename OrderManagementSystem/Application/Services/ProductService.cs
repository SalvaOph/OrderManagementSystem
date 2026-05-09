using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Application.Dtos.Product;

namespace OrderManagementSystem.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    public ProductService(IProductRepository repository) => _repository = repository;

    public async Task<List<ProductDto>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return list.Select(p => new ProductDto
        {
            ProductID = p.ProductID,
            ProductName = p.ProductName,
            UnitPrice = p.UnitPrice
        }).ToList();
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var p = await _repository.GetByIdAsync(id);
        if (p == null) return null;
        return new ProductDto
        {
            ProductID = p.ProductID,
            ProductName = p.ProductName,
            UnitPrice = p.UnitPrice
        };
    }
}
