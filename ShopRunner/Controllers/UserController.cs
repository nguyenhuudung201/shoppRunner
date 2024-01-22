using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopRunner.DatabaseContexts;
using ShopRunner.DTOs;
using ShopRunner.Entities;
using ShopRunner.Helpers;
using ShopRunner.Services.UserSetvice;
using System.Data.Entity;

using Microsoft.EntityFrameworkCore;



namespace ShopRunner.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ShopContext _context;
    private readonly IUserService _userService;

    public UserController(ShopContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }
    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllOrder()
    {
        var usersToReturn = await _userService.GetAllUser();
        return Ok(usersToReturn);
    }
    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateAdminDto dto)
    {
        // Kiem tra modelstate
        if (!ModelState.IsValid)
            return BadRequest();

        // Kiem tra xem username va password co trung khong
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToUpper().Equals(dto.Email.ToUpper()) || u.UserName.ToUpper().Equals(dto.UserName.ToUpper()));
        // Neu bi trung, tra ve 409
        if (user is not null)
            return Conflict("Email or username already exists.");

        string salt;

        var userToCreate = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Phone = dto.Phone,
            UserName = dto.UserName,
            Email = dto.Email,
            Role = dto.Role,
            Password = PasswordHelper.HashPassword(dto.Password, out salt),
            Salt = salt
        };

        _context.Users.Add(userToCreate);
        await _context.SaveChangesAsync();
        return Accepted();
    }
}
