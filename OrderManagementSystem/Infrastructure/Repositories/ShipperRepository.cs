using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Infrastructure.Data;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Infrastructure.Repositories;

public class ShipperRepository : IShipperRepository
{
    private readonly ApplicationDbContext _db;
    public ShipperRepository(ApplicationDbContext db) => _db = db;

    public async Task<List<Shipper>> GetAllAsync()
    {
        return await _db.Shippers.ToListAsync();
    }

    public async Task<Shipper?> GetByIdAsync(int id)
    {
        return await _db.Shippers.FindAsync(id);
    }
}
