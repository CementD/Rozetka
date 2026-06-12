using Domain;
using Microsoft.EntityFrameworkCore;

namespace Rozetka.BLL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _db;

        public OrderRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _db.Orders.ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _db.Orders.FindAsync(id);
        }

        public async Task<Order?> GetByIdWithDetailsAsync(int id)
        {
            return await _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetByProductIdAsync(int productId)
        {
            return await _db.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.OrderItems.Any(i => i.ProductId == productId))
                .ToListAsync();
        }

        public async Task<List<Order>> GetByUserIdAsync(string userId)
        {
            return await _db.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _db.Orders.Update(order);
            await Task.CompletedTask;
        }

        public async Task AddOrderAsync(Order order)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
        }

        public async Task<Order?> AddOrderFromCartAsync(string userId)
        {
            var cart = await _db.Carts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.Items.Any()) return null;

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = "Created"
            };

            foreach (var ci in cart.Items)
            {
                var oi = new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product?.Price ?? 0m
                };
                order.OrderItems.Add(oi);
            }

            _db.Orders.Add(order);
            _db.CartItems.RemoveRange(cart.Items);
            await _db.SaveChangesAsync();

            return order;
        }

        public async Task CancelOrderAsync(int orderId)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = "Cancelled";
                await _db.SaveChangesAsync();
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }


    }
}