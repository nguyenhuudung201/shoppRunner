using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopRunner.Entities
{
    public class OrderItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required decimal Price { get; set; }
        public required int Quanity { get; set; }
        [ForeignKey(nameof(Product))]
        public required int ProductId { get; set; }
        public Product? Product { get; set; }
        [ForeignKey(nameof(Order))]
        public required int OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
