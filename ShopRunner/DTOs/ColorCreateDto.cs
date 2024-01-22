using System.ComponentModel.DataAnnotations;

namespace ShopRunner.DTOs
{
    public class ColorCreateDto
    {
        [Required]
        [StringLength(16, MinimumLength = 3)]
        public required string Name { get; set; }
    }
}
