using System.ComponentModel.DataAnnotations;

namespace ShopRunner.DTOs;

public class ProductCreateDto
{
    [Required]
    [StringLength(16, MinimumLength = 3)]
    public required string Name { get; set; }

    [Required]
    public required string Description { get; set; }

    [Required]
    public required IFormFile Imgage { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public required decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public required int ColorId { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public required int SizeId { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public required int CategoryId { get; set; }
}
