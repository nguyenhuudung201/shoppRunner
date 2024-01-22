namespace ShopRunner.DTOs
{
    public class UserProfileUpdateDto
    {
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public string? Password { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
