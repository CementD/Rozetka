using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task DeleteAsync(int id);
        Task UpdateAsync(Product product);
    }
}
