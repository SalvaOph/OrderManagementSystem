
using OrderManagementSystem.Application.Dtos.Customer;

namespace OrderManagementSystem.Application.Interfaces;

public interface ICustomerService
{
    Task<List<CustomerDto>> GetAllAsync();
    Task<CustomerDto> GetByIdAsync(string id);
}
