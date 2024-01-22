
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopRunner.DatabaseContexts;
using ShopRunner.DTOs;
using ShopRunner.Entities;
using ShopRunner.Helpers;
using ShopRunner.Models;
using ShopRunner.Services.AccountService;
using ShopRunner.Services.TokenService;
using System.Security.Claims;

namespace ShopRunner.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ShopContext _shopContext;
    private readonly ITokenService _tokenService;
    private readonly IAccountService _accountService;

    public AuthController(ShopContext shopContext, ITokenService tokenService, IAccountService accountService)
    {
        _shopContext = shopContext ?? throw new ArgumentNullException(nameof(shopContext));
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _accountService = accountService;
    }

    [HttpPost, Route("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var user = await _shopContext.Users.FirstOrDefaultAsync(u => u.UserName.ToUpper().Equals(userLoginDto.Username.ToUpper()));
        if (user is null)
        {
            var problemDetials = new ShopRunner.Models.ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Errors = new Dictionary<string, string[]>
                 {
                     {"Credentials", new string[] {"Email or password wrong"} }
                 }
            };
            return Unauthorized(problemDetials);
        }

        if (!PasswordHelper.VerifyPassword(user, userLoginDto.Password))
        {
            var problemDetials = new ShopRunner.Models.ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Errors = new Dictionary<string, string[]>
                 {
                     {"Credentials", new string[] {"Email or password wrong"} }
                 }
            };
            return Unauthorized(problemDetials);
        }

        var claims = new List<Claim>
            {
             new Claim(ClaimTypes.Name, user.UserName),
               new Claim(ClaimTypes.Role, user.Role)
            };
        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        _shopContext.SaveChanges();
       
        return Ok(new AuthenticatedResponse { Token=accessToken,RefreshToken=refreshToken} );
    }

    [Authorize]
    [HttpGet("user")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var username = HttpContext.User.Identity!.Name; // lay username tu trong jwt
        var user = await _shopContext.Users.FirstOrDefaultAsync(u => u.UserName.ToUpper().Equals(username!.ToUpper()));
        var userGetDto = new UserGetDto
        {
            FirstName=user!.FirstName,
            LastName=user!.LastName,
            Phone=user!.Phone,
            UserName = user!.UserName,
            Email = user.Email,
            Role = user.Role
        };
        return Ok(userGetDto);
    }

    [HttpGet("logout")]
    public IActionResult LogOut()
    {
        if (Request.Cookies["accessToken"] is not null)
        {
            Response.Cookies.Delete("accessToken");
        }

        if (Request.Cookies["refreshToken"] is not null)
        {
            Response.Cookies.Delete("refreshToken");
        }

        return NoContent();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
    {
        // Kiem tra modelstate
        if (!ModelState.IsValid)
            return BadRequest();

        // Kiem tra xem username va password co trung khong
        var user = await _shopContext.Users.FirstOrDefaultAsync(u => u.Email.ToUpper().Equals(userRegisterDto.Email.ToUpper()) || u.UserName.ToUpper().Equals(userRegisterDto.UserName.ToUpper()));
        // Neu bi trung, tra ve 409
        if (user is not null)
            return Conflict("Email or username already exists.");

        string salt;

        var userToCreate = new User
        {
            FirstName=userRegisterDto.FirstName,
            LastName=userRegisterDto.LastName,
            Phone=userRegisterDto.Phone,
            UserName = userRegisterDto.UserName,
            Email = userRegisterDto.Email,
            Role = "User",
            Password = PasswordHelper.HashPassword(userRegisterDto.Password, out salt),
            Salt = salt
        };

        _shopContext.Users.Add(userToCreate);
        await _shopContext.SaveChangesAsync();
        return Accepted();
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UserProfileUpdateDto userProfileUpdateDto)
    {
        var username = HttpContext.User.Identity!.Name; // lay username tu trong jwt
        var user = await _shopContext.Users.FirstOrDefaultAsync(u => u.UserName.ToUpper().Equals(username!.ToUpper()));
        await _accountService.UpdateProfile(user!.Id, userProfileUpdateDto);
        return NoContent();
    }
}





