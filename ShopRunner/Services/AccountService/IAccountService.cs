using ShopRunner.DTOs;

namespace ShopRunner.Services.AccountService
{
    public interface IAccountService
    {
        Task UpdateProfile(long userId, UserProfileUpdateDto userProfileUpdateDto);
    }
}
