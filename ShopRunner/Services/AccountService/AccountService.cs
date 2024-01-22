using ShopRunner.DatabaseContexts;
using ShopRunner.DTOs;
using ShopRunner.Helpers;

namespace ShopRunner.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly ShopContext _context;

        public AccountService(ShopContext context)
        {
            _context = context;
        }

        public async Task UpdateProfile(long userId, UserProfileUpdateDto userProfileUpdateDto)
        {
            var user = await _context.Users.FindAsync(userId) ?? throw new ArgumentException(null, nameof(userId));
            user.Phone = userProfileUpdateDto.Phone;
            user.Email = userProfileUpdateDto.Email;
            user.FirstName = userProfileUpdateDto.FirstName;
            user.LastName = userProfileUpdateDto.LastName;

            if (userProfileUpdateDto.Password is not null)
            {
                var newHashPassword = PasswordHelper.HashPassword(userProfileUpdateDto.Password, out var salt);
                user.Password = newHashPassword;
                user.Salt = salt;
            }
           
            await _context.SaveChangesAsync();
        }
    }
}
