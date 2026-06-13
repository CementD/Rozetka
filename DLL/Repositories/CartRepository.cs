using Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DLL.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _db;

        public CartRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Cart> GetUserCartAsync(string userId)
        {
            var cart = await _db.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _db.Carts.Add(cart);
                await _db.SaveChangesAsync();
            }

            return cart;
        }

        public async Task AddToCartAsync(string userId, int productId)
        {
            var cart = await GetUserCartAsync(userId);
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
                item.Quantity += 1;
            else
                cart.Items.Add(new CartItem { ProductId = productId, Quantity = 1, CartId = cart.Id });

            await _db.SaveChangesAsync();
        }

        public async Task UpdateQuantityAsync(int cartItemId, int delta)
        {
            var item = await _db.CartItems.FindAsync(cartItemId);
            if (item == null) return;

            item.Quantity += delta;

            if (item.Quantity <= 0)
                _db.CartItems.Remove(item);

            await _db.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(int cartItemId)
        {
            var item = await _db.CartItems.FindAsync(cartItemId);
            if (item != null)
            {
                _db.CartItems.Remove(item);
                await _db.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await _db.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                _db.CartItems.RemoveRange(cart.Items);
                await _db.SaveChangesAsync();
            }
        }
    }
}
