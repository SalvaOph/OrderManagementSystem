using OrderManagementSystem.Models;

namespace OrderManagementSystem.Application.Interfaces;

public interface IShipperRepository
{
    Task<List<Shipper>> GetAllAsync();
    Task<Shipper?> GetByIdAsync(int id);
}
