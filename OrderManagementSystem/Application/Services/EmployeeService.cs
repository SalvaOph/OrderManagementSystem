using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Application.Dtos.Employee;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;
    public EmployeeService(IEmployeeRepository repository) => _repository = repository;

    public async Task<List<EmployeeDto>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return list.Select(e => new EmployeeDto
        {
            EmployeeID = e.EmployeeID,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Title = e.Title
        }).ToList();
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        var e = await _repository.GetByIdAsync(id);
        if (e == null) return null;
        return new EmployeeDto
        {
            EmployeeID = e.EmployeeID,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Title = e.Title
        };
    }
}
