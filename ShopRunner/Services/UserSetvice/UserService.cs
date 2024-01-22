using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopRunner.DatabaseContexts;
using ShopRunner.DTOs;
using ShopRunner.Entities;
using ShopRunner.Helpers;
using ShopRunner.Utilities;
using System.Runtime.CompilerServices;

namespace ShopRunner.Services.UserSetvice;

public class UserService: IUserService
{
    private readonly ShopContext _context;

    public UserService(ShopContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<ListUserDtos>> GetAllUser()
    {
        var users = await _context.Users.ToListAsync();
        var usersToReturn = users.Select(u => new ListUserDtos
        {
            UserId= u.Id,
            FirstName=u.FirstName,  
            LastName=u.LastName,    
            Email=u.Email,
            Phone=u.Phone,
            UserName=u.UserName,
            Role=u.Role,

        });
        return usersToReturn;
    }
  
   
}
