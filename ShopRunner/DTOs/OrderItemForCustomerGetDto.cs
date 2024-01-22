namespace ShopRunner.DTOs;

public class OrderItemForCustomerGetDto
{
    public required string ProductName { get; set; }
    public required string ProductImage { get; set; }
    public required decimal Price { get; set; }
    public required int Quantity { get; set; }
}
