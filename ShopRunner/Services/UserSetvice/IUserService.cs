using ShopRunner.DTOs;

namespace ShopRunner.Services.UserSetvice
{
    public interface IUserService
    {
        Task<IEnumerable<ListUserDtos>> GetAllUser();
       
    }
}
