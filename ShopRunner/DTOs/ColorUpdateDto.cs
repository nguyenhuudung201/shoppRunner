using System.ComponentModel.DataAnnotations;

namespace ShopRunner.DTOs
{
    public class ColorUpdateDto
    {
        [Required]
        [StringLength(16, MinimumLength = 3)]
        public required string Name { get; set; }
    }
}
