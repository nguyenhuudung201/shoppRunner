using System.ComponentModel.DataAnnotations.Schema;

namespace ShopRunner.DTOs
{
    public class ItemsCreateDto
    {
        public int Id { get; set; }
        public required decimal Price { get; set; }
        public required int Quanity { get; set; }
    
        public required int ProductId { get; set; }
    
        public required int OrderId { get; set; }
     
    }
}
