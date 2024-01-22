namespace ShopRunner.DTOs;

public class OrderItemGetDto
{
    public required string ProductName { get; set; }
    public required decimal Price { get; set; }
    public required int Quanity { get; set; }
}