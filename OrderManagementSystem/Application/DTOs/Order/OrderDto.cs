namespace OrderManagementSystem.Application.Dtos.Order;

public class OrderDto
{
    public int OrderID { get; set; }
    public string? CustomerID { get; set; }
    public int EmployeeID { get; set; }
    public int? ShipVia { get; set; }
    public DateTime? OrderDate { get; set; }

    public string? ShipAddress { get; set; }
    public string? ShipCity { get; set; }
}
