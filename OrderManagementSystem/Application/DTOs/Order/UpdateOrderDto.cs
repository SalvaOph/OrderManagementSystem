using OrderManagementSystem.Application.Dtos.OrderDetail;

namespace OrderManagementSystem.Application.Dtos.Order;

public class UpdateOrderDto
{
    public string? CustomerID { get; set; }
    public int EmployeeID { get; set; }
    public int? ShipVia { get; set; }
    public DateTime? OrderDate { get; set; }

    public string? ShipAddress { get; set; }
    public string? ShipCity { get; set; }
    public string? ShipRegion { get; set; }
    public string? ShipPostalCode { get; set; }
    public string? ShipCountry { get; set; }
    public List<UpdateOrderDetailDto>? OrderDetails { get; set; }
}
