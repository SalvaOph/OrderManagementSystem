namespace OrderManagementSystem.Models;

public class Order
{
        public int OrderID { get; set; }
        public string? CustomerID { get; set; }
        public int EmployeeID { get; set; }
        public int? ShipVia { get; set; }
        public decimal Freight { get; set; }
        public string? ShipName { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipRegion { get; set; }
        public string? ShipPostalCode { get; set; }
        public string? ShipCountry { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }

    public virtual Customer Customer { get; set; } = null!;
    public virtual Employee Employee { get; set; } = null!;
    public virtual Shipper? Shipper { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
