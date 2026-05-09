using OrderManagementSystem.Application.Dtos.Shipper;

namespace OrderManagementSystem.Application.Interfaces;

public interface IShipperService
{
    Task<List<ShipperDto>> GetAllAsync();
    Task<ShipperDto?> GetByIdAsync(int id);
}
