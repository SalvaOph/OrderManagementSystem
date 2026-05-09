namespace OrderManagementSystem.Models;

public class Customer
{
    public string CustomerID { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public string? ContactName { get; set; } = null!;
    public string? ContactTitle { get; set; } = null!;
    public string? Address { get; set; } = null!;
    public string? City { get; set; } = null!;
    public string? Region { get; set; } = null!;
    public string? PostalCode { get; set; } = null!;
    public string? Country { get; set; } = null!;
    public string? Phone { get; set; } = null!;
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}