namespace OrderManagementSystem.Application.Dtos.OrderDetail;

public class UpdateOrderDetailDto
{
    public int ProductID { get; set; }
    public decimal UnitPrice { get; set; }
    public short Quantity { get; set; }
    public float Discount { get; set; }
}
