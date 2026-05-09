using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Application.Dtos.Order;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    public OrderService(IOrderRepository repository) => _repository = repository;

    public async Task<List<OrderDto>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return list.Select(o => new OrderDto
        {
            OrderID = o.OrderID,
            CustomerID = o.CustomerID,
            EmployeeID = o.EmployeeID,
            ShipVia = o.ShipVia,
            OrderDate = o.OrderDate,
            ShipAddress = o.ShipAddress,
            ShipCity = o.ShipCity
        }).ToList();
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var o = await _repository.GetByIdAsync(id);
        if (o == null) return null;
        return new OrderDto
        {
            OrderID = o.OrderID,
            CustomerID = o.CustomerID,
            EmployeeID = o.EmployeeID,
            ShipVia = o.ShipVia,
            OrderDate = o.OrderDate,
            ShipAddress = o.ShipAddress,
            ShipCity = o.ShipCity
        };
    }

    public async Task CreateAsync(CreateOrderDto dto)
    {
        var order = new Order
        {
            CustomerID = dto.CustomerID,
            EmployeeID = dto.EmployeeID,
            ShipVia = dto.ShipVia,
            OrderDate = dto.OrderDate ?? DateTime.Now,
            ShipAddress = dto.ShipAddress,
            ShipCity = dto.ShipCity,
            ShipRegion = dto.ShipRegion,
            ShipPostalCode = dto.ShipPostalCode,
            ShipCountry = dto.ShipCountry,
            OrderDetails = new List<OrderDetail>()
        };

        // Map order details from DTO to the Order's navigation property so EF Core
        // will persist them together with the Order via the relationship.
        if (dto.OrderDetails != null && dto.OrderDetails.Any())
        {
            foreach (var d in dto.OrderDetails)
            {
                var od = new OrderDetail
                {
                    ProductID = d.ProductID,
                    UnitPrice = d.UnitPrice,
                    Quantity = d.Quantity,
                    Discount = d.Discount
                };

                order.OrderDetails.Add(od);
            }
        }

        await _repository.CreateAsync(order);
    }

    public async Task UpdateAsync(int id, UpdateOrderDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return;

        existing.CustomerID = dto.CustomerID;
        existing.EmployeeID = dto.EmployeeID;
        existing.ShipVia = dto.ShipVia;
        existing.OrderDate = dto.OrderDate ?? existing.OrderDate;
        existing.ShipAddress = dto.ShipAddress;
        existing.ShipCity = dto.ShipCity;
        existing.ShipRegion = dto.ShipRegion;
        existing.ShipPostalCode = dto.ShipPostalCode;
        existing.ShipCountry = dto.ShipCountry;
        existing.OrderDetails = dto.OrderDetails?.Select(d => new OrderDetail
        {
            ProductID = d.ProductID,
            UnitPrice = d.UnitPrice,
            Quantity = d.Quantity,
            Discount = d.Discount
        }).ToList() ?? existing.OrderDetails;

        await _repository.UpdateAsync(existing);
    }
}
