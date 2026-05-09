namespace OrderManagementSystem.Models;

public class Shipper
{
    public int ShipperID { get; set; }
    public string CompanyName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}