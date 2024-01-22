using System.ComponentModel.DataAnnotations;

namespace ShopRunner.DTOs
{
    public class OrderItemCreateDto
    {
        [Required]
        public required decimal Price { get; set; }

        [Required]
        public required int Quanity { get; set; }

        [Required]
        public required int ProductId { get; set; }
    }
}
