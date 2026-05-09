using OrderManagementSystem.Models;

namespace OrderManagementSystem.Application.Interfaces;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(string id);
}
