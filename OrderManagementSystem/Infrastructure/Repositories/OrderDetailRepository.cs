using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Infrastructure.Data;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Infrastructure.Repositories;

public class OrderDetailRepository : IOrderDetailRepository
{
    private readonly ApplicationDbContext _db;
    public OrderDetailRepository(ApplicationDbContext db) => _db = db;

    public async Task<List<OrderDetail>> GetAllAsync()
    {
        return await _db.OrderDetails
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<OrderDetail?> GetByIdAsync(int orderId, int productId)
    {
        return await _db.OrderDetails
            .Where(od => od.OrderID == orderId && od.ProductID == productId)
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync(OrderDetail orderDetail)
    {
        _db.OrderDetails.Add(orderDetail);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(OrderDetail orderDetail)
    {
        _db.OrderDetails.Update(orderDetail);
        await _db.SaveChangesAsync();
    }
}
