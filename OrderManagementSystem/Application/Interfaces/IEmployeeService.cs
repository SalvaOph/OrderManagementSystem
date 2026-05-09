using OrderManagementSystem.Application.Dtos.Employee;

namespace OrderManagementSystem.Application.Interfaces;

public interface IEmployeeService
{
    Task<List<EmployeeDto>> GetAllAsync();
    Task<EmployeeDto?> GetByIdAsync(int id);
}
