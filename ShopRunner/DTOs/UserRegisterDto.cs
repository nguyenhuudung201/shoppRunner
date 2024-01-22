using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopRunner.DTOs
{
    public class UserRegisterDto

    {
        [Required]
        [StringLength(32, MinimumLength = 3)]
        public required string FirstName { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 3)]
        public required string LastName { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 10)]
        public required string Phone { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 3)]
        public required string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(256)]
        public required string Email { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
