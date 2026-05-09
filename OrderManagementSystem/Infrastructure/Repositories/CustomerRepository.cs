using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Infrastructure.Data;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _db;
    public CustomerRepository(ApplicationDbContext db) => _db = db;

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _db.Customers.ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(string id)
    {
        return await _db.Customers.FindAsync(id);
    }
}
