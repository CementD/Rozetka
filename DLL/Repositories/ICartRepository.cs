using Domain;
using System.Threading.Tasks;

namespace DLL.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetUserCartAsync(string userId);
        Task AddToCartAsync(string userId, int productId);
        Task UpdateQuantityAsync(int cartItemId, int delta);
        Task RemoveItemAsync(int cartItemId);
        Task ClearCartAsync(string userId);
    }
}
