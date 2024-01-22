﻿using System.ComponentModel.DataAnnotations;

namespace ShopRunner.DTOs
{
    public class CategoryUpdateDto
    {
        [Required]
        [StringLength(16, MinimumLength = 3)]
        public required string Name { get; set; }

        [Required]
        public required IFormFile Image { get; set; }
    }
}
