namespace ShopRunner.DTOs;

public class OrderForCustomerGetDto
{
    public required int Id { get; set; }    
    public required DateTime? CreatedAt { get; set; }
    public required decimal Total { get; set; }
    public required string Status { get; set; }
}
