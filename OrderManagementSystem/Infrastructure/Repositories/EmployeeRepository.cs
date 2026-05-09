using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Infrastructure.Data;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _db;
    public EmployeeRepository(ApplicationDbContext db) => _db = db;

    public async Task<List<Employee>> GetAllAsync()
    {
        return await _db.Employees.ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _db.Employees.FindAsync(id);
    }
}
