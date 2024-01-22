namespace ShopRunner.DTOs;

public class OrderGetDto
{
    public required int Id { get; set; }
    public required decimal Total { get; set; }
    public required string CustomerName { get; set; }

    public required IEnumerable<OrderItemGetDto> OrderItems { get; set; }
}
