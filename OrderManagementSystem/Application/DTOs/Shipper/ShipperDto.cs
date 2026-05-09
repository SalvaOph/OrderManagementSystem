namespace OrderManagementSystem.Application.Dtos.Shipper;

public class ShipperDto
{
    public int ShipperID { get; set; }
    public string CompanyName { get; set; } = null!;
    public string Phone { get; set; } = null!;
}