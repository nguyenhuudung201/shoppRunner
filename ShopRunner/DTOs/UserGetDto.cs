using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopRunner.DTOs
{
    public class UserGetDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Phone { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
    }
}
