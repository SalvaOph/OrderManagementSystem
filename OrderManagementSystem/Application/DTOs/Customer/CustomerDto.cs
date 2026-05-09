namespace OrderManagementSystem.Application.Dtos.Customer;
public class CustomerDto
{
    public string CustomerID { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public string? City { get; set; }
    public string? Country { get; set; }
}
