namespace OrderManagementSystem.Application.Dtos.OrderDetail;

public class CreateOrderDetailDto
{
    public int OrderID { get; set; }
    public int ProductID { get; set; }
    public decimal UnitPrice { get; set; }
    public short Quantity { get; set; }
    public float Discount { get; set; }
}
