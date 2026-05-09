namespace OrderManagementSystem.Application.Dtos.Product;

public class ProductDto
{
    public int ProductID { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal UnitPrice { get; set; }
}
