using Microsoft.AspNetCore.Mvc;
using ShopRunner.DTOs;

namespace ShopRunner.Services.OrderService
{
    public interface IOrderService
    {
        Task<OrderGetDto> Create(OrderCreateDto dto, long userId);
        Task<IEnumerable<OrderForCustomerGetDto>> GetOrdersForCustomerAsync(long userId);
        Task<OrderDetailForCustomerGetDto> GetOrderDetailAsync(int orderId);
        Task<IEnumerable<OrderForCustomerGetDto>> GetAllOrder();
    }
}
