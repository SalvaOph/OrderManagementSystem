using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Application.Dtos.Customer;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    public CustomerService(ICustomerRepository repository) => _repository = repository;

    public async Task<List<CustomerDto>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return list.Select(c => new CustomerDto
        {
            CustomerID = c.CustomerID,
            CompanyName = c.CompanyName,
            City = c.City,
            Country = c.Country
        }).ToList();
    }

    public async Task<CustomerDto> GetByIdAsync(string id)
    {
        var c = await _repository.GetByIdAsync(id.ToString());
        if (c == null) return null;

        return new CustomerDto
        {
            CustomerID = c.CustomerID,
            CompanyName = c.CompanyName,
            City = c.City,
            Country = c.Country
        };
    }
}
