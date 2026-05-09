using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Infrastructure.Data;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _db;
    public OrderRepository(ApplicationDbContext db) => _db = db;

    public async Task<List<Order>> GetAllAsync()
    {
        return await _db.Orders
            .Include(o => o.OrderDetails)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _db.Orders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.OrderID == id);
    }

    public async Task CreateAsync(Order order)
    {
        // Adding the Order with populated OrderDetails (navigation) will let EF Core
        // insert the Order first, then the dependent OrderDetails using the generated OrderID.
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        _db.Orders.Update(order);
        await _db.SaveChangesAsync();
    }
}
