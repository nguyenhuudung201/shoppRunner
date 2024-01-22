using Microsoft.EntityFrameworkCore;
using ShopRunner.DatabaseContexts;
using ShopRunner.DTOs;
using ShopRunner.Entities;
using ShopRunner.Utilities;
using System.Runtime.CompilerServices;

namespace ShopRunner.Services.OrderService;

public class OrderServicecs : IOrderService
{
    private readonly ShopContext _context;

    public OrderServicecs(ShopContext context)
    {
        _context = context;
    }

    public async Task<OrderGetDto> Create(OrderCreateDto dto, long userId)
    {
        var customer = await _context.Users
            .FindAsync(userId);

        // Create Order
        var orderToCreate = new Order
        {           
            Total = dto.Total,
            UserId = userId,    
            HomeNumber = dto.HomeNumber,
            Quarter = dto.Quarter,
            City = dto.City,
            Country = dto.Country
        };

        _context.Orders.Add(orderToCreate);
        await _context.SaveChangesAsync();

        // Create OrderItems
        var orderItemsToCreate = dto.OrderItems.Select(oi => new OrderItems
        {
            OrderId = orderToCreate.Id,
            Quanity = oi.Quanity,
            ProductId = oi.ProductId,
            Price = oi.Price
        });
        _context.Items.AddRange(orderItemsToCreate);
        await _context.SaveChangesAsync();

        var orderItems = await _context.Items
            .AsNoTracking()
            .Include(oi => oi.Product)
            .Where(oi => oi.OrderId == orderToCreate.Id)
            .ToListAsync();

        var orderItemsToReturn = orderItems.Select(oi => new OrderItemGetDto
        {
            Quanity = oi.Quanity,
            ProductName = oi.Product!.Name,
            Price = oi.Price
        });

        var orderToReturn = new OrderGetDto
        {
            CustomerName = (await _context.Users.FindAsync(userId) ?? throw new ArgumentException(null, nameof(userId))).UserName,
            Id = orderToCreate.Id,
            Total = orderToCreate.Total,
            OrderItems = orderItemsToReturn

        };
        return orderToReturn;
    }

    public async Task<IEnumerable<OrderForCustomerGetDto>> GetOrdersForCustomerAsync(long userId)
    {
        var orders = await _context.Orders
            .AsNoTracking()
            .Include("Items")
            .Include("Items.Product")
            .Where(o => o.UserId == userId)
            .ToListAsync();

        var ordersToReturn = orders.Select(o => new OrderForCustomerGetDto
        {
            Id = o.Id,
            Total = o.Total,
            Status = GetOrderStatusAsString(o.OrderStatus),
            CreatedAt = o.CreatedAt
        });

        return ordersToReturn;
    }
    public async Task<IEnumerable<OrderForCustomerGetDto>> GetAllOrder()
    {
        var orders = await _context.Orders
            .AsNoTracking()
            .Include("Items")
            .Include("Items.Product")
            .ToListAsync();

        var ordersToReturn = orders.Select(o => new OrderForCustomerGetDto
        {
            Id = o.Id,
            Total = o.Total,
            Status = GetOrderStatusAsString(o.OrderStatus),
            CreatedAt = o.CreatedAt
        });

        return ordersToReturn;
    }

    public async Task<OrderDetailForCustomerGetDto> GetOrderDetailAsync(int orderId)
    {
        var orderItems = await _context.Items
            .AsNoTracking()
            .Include(oi => oi.Product)
            .Where(oi => oi.OrderId == orderId)
            .ToListAsync();

        var order = await _context.Orders
            .FindAsync(orderId) ?? throw new ArgumentException(null, nameof(orderId));

        var orderDetailToReturn = new OrderDetailForCustomerGetDto
        {
            Id = order.Id,
            Total = order.Total,
            CreatedAt = order.CreatedAt,
            Status = GetOrderStatusAsString(order.OrderStatus),
            OrderItems = orderItems.Select(oi => new OrderItemForCustomerGetDto
            {
                Price = oi.Price,
                Quantity = oi.Quanity,
                ProductImage = oi.Product!.Image,
                ProductName = oi.Product.Name
            })
        };

        return orderDetailToReturn;
    }

    private string GetOrderStatusAsString(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Processing => "Processing",
            OrderStatus.Shipping => "Shipping",
            OrderStatus.Completed => "Completed",
            _ => throw new SwitchExpressionException()
        };
    }
}
