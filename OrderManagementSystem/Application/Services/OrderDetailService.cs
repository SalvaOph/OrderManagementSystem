using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Application.Dtos.OrderDetail;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Application.Services;

public class OrderDetailService : IOrderDetailService
{
    private readonly IOrderDetailRepository _repository;
    private readonly IOrderRepository _orderRepo;
    private readonly IProductRepository _productRepo;

    public OrderDetailService(IOrderDetailRepository repository, IOrderRepository orderRepo, IProductRepository productRepo)
    {
        _repository = repository;
        _orderRepo = orderRepo;
        _productRepo = productRepo;
    }

    public async Task<List<OrderDetailDto>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return list.Select(od => new OrderDetailDto
        {
            ProductID = od.ProductID,
            UnitPrice = od.UnitPrice,
            Quantity = od.Quantity,
            Discount = od.Discount
        }).ToList();
    }

    public async Task<OrderDetailDto?> GetByIdAsync(int orderId, int productId)
    {
        var od = await _repository.GetByIdAsync(orderId, productId);
        if (od == null) return null;
        return new OrderDetailDto
        {
            ProductID = od.ProductID,
            UnitPrice = od.UnitPrice,
            Quantity = od.Quantity,
            Discount = od.Discount
        };
    }

    public async Task CreateAsync(CreateOrderDetailDto dto)
    {
        // Basic business rules: referenced Order and Product must exist
        var order = await _orderRepo.GetByIdAsync(dto.OrderID);
        if (order == null) throw new InvalidOperationException("Order not found");

        var product = await _productRepo.GetByIdAsync(dto.ProductID);
        if (product == null) throw new InvalidOperationException("Product not found");

        var existing = await _repository.GetByIdAsync(dto.OrderID, dto.ProductID);
        if (existing != null) throw new InvalidOperationException("OrderDetail already exists");

        var od = new OrderDetail
        {
            OrderID = dto.OrderID,
            ProductID = dto.ProductID,
            UnitPrice = dto.UnitPrice,
            Quantity = dto.Quantity,
            Discount = dto.Discount
        };

        await _repository.CreateAsync(od);
    }

    public async Task UpdateAsync(int orderId, int productId, UpdateOrderDetailDto dto)
    {
        var existing = await _repository.GetByIdAsync(orderId, productId);
        if (existing == null) throw new InvalidOperationException("OrderDetail not found");

        existing.UnitPrice = dto.UnitPrice;
        existing.Quantity = dto.Quantity;
        existing.Discount = dto.Discount;

        await _repository.UpdateAsync(existing);
    }
}
