using OrderManagementSystem.Application.Interfaces;
using OrderManagementSystem.Application.Dtos.Shipper;

namespace OrderManagementSystem.Application.Services;

public class ShipperService : IShipperService
{
    private readonly IShipperRepository _repository;
    public ShipperService(IShipperRepository repository) => _repository = repository;

    public async Task<List<ShipperDto>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return list.Select(s => new ShipperDto
        {
            ShipperID = s.ShipperID,
            CompanyName = s.CompanyName,
            Phone = s.Phone
        }).ToList();
    }

    public async Task<ShipperDto?> GetByIdAsync(int id)
    {
        var s = await _repository.GetByIdAsync(id);
        if (s == null) return null;
        return new ShipperDto
        {
            ShipperID = s.ShipperID,
            CompanyName = s.CompanyName,
            Phone = s.Phone
        };
    }
}
