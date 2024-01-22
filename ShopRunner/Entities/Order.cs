using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ShopRunner.Utilities;

namespace ShopRunner.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }     
        [Required]
        public required string HomeNumber { get; set; }
        [Required]
        public required string Quarter { get;set; }
        [Required]
        public required string Country { get; set; }
        [Required]
        public required string City { get; set; }
        [Required]
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Processing;
        public required decimal Total { get; set; }
        [ForeignKey(nameof(User))]
        public required long UserId { get; set; }
        public User? User { get; set; }
        public ICollection<OrderItems>? Items { get; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
