using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Application.Dtos.OrderDetail;
using OrderManagementSystem.Application.DTOs.OrderDetailViews;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Infrastructure.Data;

namespace OrderManagementSystem.Infrastructure.Repositories;

public class OrderDetailViewRepository : IOrderDetailViewRepository
{
    private readonly ApplicationDbContext _db;
    public OrderDetailViewRepository(ApplicationDbContext db) => _db = db;
    public async Task<List<OrderDetailViewDto>> GetAllAsync()
    {
        return await _db.Orders
            .Where(o => o.OrderDate.HasValue)
            .Select(o => new OrderDetailViewDto
            {
                OrderID = o.OrderID,
                CustomerName = o.Customer.CompanyName,
                CustomerID = o.CustomerID,
                EmployeeID = o.EmployeeID,
                Freight = o.Freight,
                ShipVia = o.ShipVia,
                ShipAddress = o.ShipAddress,
                OrderDate = o.OrderDate!.Value,
                Region = o.ShipRegion,
                ShipName = o.ShipName,
                ShipCity = o.ShipCity,
                ShipRegion = o.ShipRegion,
                ShipPostalCode = o.ShipPostalCode,
                ShipCountry = o.ShipCountry,
                RequiredDate = o.RequiredDate,
                ShippedDate = o.ShippedDate,
                Products = o.OrderDetails.Select(od => od.Product.ProductName).ToList(),
                ProductDetails = o.OrderDetails.Select(od => new OrderDetailDto
                {
                    ProductID = od.ProductID,
                    UnitPrice = od.UnitPrice,
                    Quantity = od.Quantity,
                }).ToList(),
                TotalAmount = o.OrderDetails.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount))
            })
            .ToListAsync();
    }

    public async Task<OrderDetailViewDto?> GetByIdAsync(int orderId)
    {
        var o = await _db.Orders
            .Where(o => o.OrderID == orderId && o.OrderDate.HasValue)
            .Select(o => new OrderDetailViewDto
            {
                OrderID = o.OrderID,
                CustomerName = o.Customer.CompanyName,
                CustomerID = o.CustomerID,
                EmployeeID = o.EmployeeID,
                Freight = o.Freight,
                ShipVia = o.ShipVia,
                ShipAddress = o.ShipAddress,
                OrderDate = o.OrderDate!.Value,
                Region = o.ShipRegion,
                ShipName = o.ShipName,
                ShipCity = o.ShipCity,
                ShipRegion = o.ShipRegion,
                ShipPostalCode = o.ShipPostalCode,
                ShipCountry = o.ShipCountry,
                RequiredDate = o.RequiredDate,
                ShippedDate = o.ShippedDate,
                Products = o.OrderDetails.Select(od => od.Product.ProductName).ToList(),
                ProductDetails = o.OrderDetails.Select(od => new OrderDetailDto
                {
                    ProductID = od.ProductID,
                    UnitPrice = od.UnitPrice,
                    Quantity = od.Quantity,
                }).ToList(),
                TotalAmount = o.OrderDetails.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount))
            })
            .FirstOrDefaultAsync();

        return o;
    }
}
