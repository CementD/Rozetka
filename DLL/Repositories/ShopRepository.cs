using Domain;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repositories
{
    public class ShopRepository : IShopRepository
    {
        private readonly AppDbContext _db;

        public ShopRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Shop?> GetByIdAsync(int id)
        {
            return await _db.Shops
                .Include(s => s.Owner)
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Shop?> GetByOwnerIdAsync(string ownerId)
        {
            return await _db.Shops
                .FirstOrDefaultAsync(s => s.OwnerId == ownerId);
        }

        public async Task<IEnumerable<Shop>> GetPendingShopsAsync()
        {
            return await _db.Shops
                .Include(s => s.Owner)
                .Where(s => !s.IsApproved)
                .ToListAsync();
        }

        public async Task AddAsync(Shop shop)
        {
            await _db.Shops.AddAsync(shop);
        }

        public void Update(Shop shop)
        {
            _db.Shops.Update(shop);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Shop>> GetAllShopsAsync()
        {
            return await _db.Shops
                .Where(s => s.IsApproved == true)
                .Include(s => s.Owner)
                .Include(s => s.Products)
                .ToListAsync();
        }
    }
}