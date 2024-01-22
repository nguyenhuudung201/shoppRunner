namespace ShopRunner.DTOs
{
    public class ProductGetDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Image { get; set; }
        public required decimal Price { get; set; }
        
        public required string Color { get; set; }
        public required int ColorId { get; set; }
        public required string Size { get; set;}
        public required int SizeId { get; set; }
        public required string Category { get; set; }
        public required int CategoryId { get; set; }
    }
}
