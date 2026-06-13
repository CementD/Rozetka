using Domain;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace DLL.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();

        Task<Order?> GetByIdAsync(int id);

        Task<Order?> GetByIdWithDetailsAsync(int id);

        Task<List<Order>> GetByProductIdAsync(int productId);

        Task<List<Order>> GetByUserIdAsync(string userId);

        Task AddOrderAsync(Order order);

        Task<Order?> AddOrderFromCartAsync(string userId);

        Task CancelOrderAsync(int orderId);

        Task UpdateAsync(Order order);

        Task<bool> SaveChangesAsync();
    }
}
