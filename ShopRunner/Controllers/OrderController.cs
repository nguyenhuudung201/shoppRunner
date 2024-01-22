
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopRunner.DatabaseContexts;
using ShopRunner.DTOs;
using ShopRunner.Services.OrderService;

namespace ShopRunner.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly ShopContext _context;
    private readonly IOrderService _orderService;

    public OrderController(ShopContext context, IOrderService orderService)
    {
        _context = context;
        _orderService = orderService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
    {
        var userId = await GetUserId(HttpContext);
        var orderToReturn = await _orderService.Create(dto, userId);

        return Created("", orderToReturn);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetOrdersForCustomer()
    {
        var userId = await GetUserId(HttpContext);
        var ordersToReturn = await _orderService.GetOrdersForCustomerAsync(userId);
        return Ok(ordersToReturn);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("order-admin")]
    public async Task<IActionResult> GetAllOrder()
    {
        var ordersToReturn = await _orderService.GetAllOrder();
        return Ok(ordersToReturn);
    }
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrderDetail([FromRoute] int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order is null) return NotFound();
        var orderDetailToReturn = await _orderService.GetOrderDetailAsync(id);
        return Ok(orderDetailToReturn);
    }
    [Authorize(Roles = "admin")]
    [HttpGet("order-admin/{id:int}")]
    public async Task<IActionResult> GetOrderDetailAdmin([FromRoute] int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order is null) return NotFound();
        var orderDetailToReturn = await _orderService.GetOrderDetailAsync(id);
        return Ok(orderDetailToReturn);
    }
    private async Task<long> GetUserId(HttpContext httpContext)
    {
        var username = HttpContext.User.Identity!.Name; // lay username tu trong jwt
        var userId = (await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserName == username) ?? throw new Exception()).Id;
        return userId;
    }

}