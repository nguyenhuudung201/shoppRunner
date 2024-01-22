namespace ShopRunner.Models;

public class RequestParamsForProduct : RequestParams
{
    public string? Price { get; set; }
    public int? SizeId { get; set; }
    public int? ColorId { get; set; }
    public int? CategoryId { get; set; }    
}
