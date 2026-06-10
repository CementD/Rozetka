using Domain;

namespace Rozetka.BLL.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();

        Task<Order?> GetByIdAsync(int id);

        Task<Order?> GetByIdWithDetailsAsync(int id);

        Task<List<Order>> GetByProductIdAsync(int productId);

        Task<List<Order>> GetByUserIdAsync(string userId);

        Task UpdateAsync(Order order);

        Task<bool> SaveChangesAsync();
    }
}