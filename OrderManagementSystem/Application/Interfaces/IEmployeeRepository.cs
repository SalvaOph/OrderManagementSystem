using OrderManagementSystem.Models;

namespace OrderManagementSystem.Application.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);
}
