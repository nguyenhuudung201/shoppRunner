namespace ShopRunner.DTOs;

public class OrderDetailForCustomerGetDto : OrderForCustomerGetDto
{
    public required IEnumerable<OrderItemForCustomerGetDto>? OrderItems { get; set; }
}
