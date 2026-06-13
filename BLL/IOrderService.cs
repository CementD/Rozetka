using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DTOs;

namespace BLL
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderReadDto>> GetAllOrdersAsync();
        Task<OrderDetailsDto?> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderReadDto>> GetOrdersByUserAsync(string userId);
        Task<OrderDetailsDto?> CreateOrderFromCartAsync(string userId);
        Task UpdateOrderStatusAsync(OrderUpdateStatusDto dto);
        Task CancelOrderAsync(int id);
    }
}