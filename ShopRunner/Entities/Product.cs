using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopRunner.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Image { get; set; }
        public required decimal Price { get; set; } 
        [ForeignKey(nameof(Color))]
        public required int ColorId { get; set; }
        public Color? Color { get; set; }
        [ForeignKey(nameof(Size))]
        public required int SizeId { get; set;}
        public Size? Size { get; set; }

        [ForeignKey (nameof(Category))]
        public required int  CategoryId { get; set;}
        public Category? Category { get; set;}
    }
}
