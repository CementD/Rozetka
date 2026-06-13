using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _db;

        public UserRepository(UserManager<User> userManager, AppDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<User?> GetByIdWithOrdersAsync(string id)
        {
            return await _db.Users
                .Include(u => u.Orders)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}