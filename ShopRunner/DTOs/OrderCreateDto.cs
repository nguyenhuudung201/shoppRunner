using ShopRunner.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopRunner.DTOs
{
    public class OrderCreateDto
    {
        [Required]
        public required IEnumerable<OrderItemCreateDto> OrderItems { get; set; }
        [Required]
        public required decimal Total { get; set; }
        [Required]
        public required string HomeNumber { get; set; }
        [Required]
        public required string Quarter { get; set; }
        [Required]
        public required string Country { get; set; }
        [Required]
        public required string City { get; set; }
    }
}
