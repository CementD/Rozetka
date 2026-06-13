using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace DLL.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task DeleteAsync(int id);
        Task UpdateAsync(Product product);
        Task<List<Order>> GetProductOrdersAsync(int productId);
        Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId);
    }
}
