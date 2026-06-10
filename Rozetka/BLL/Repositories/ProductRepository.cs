using Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;
        public ProductRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Products.Include(p => p.Category).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Product product)
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Order>> GetProductOrdersAsync(int productId)
        {
            return await _db.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.OrderItems.Any(oi => oi.ProductId == productId))
                .ToListAsync();
        }

        public Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return _db.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }
    }
}
