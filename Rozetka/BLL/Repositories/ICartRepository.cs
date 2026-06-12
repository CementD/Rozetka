using Domain;
using Domain;
using System.Threading.Tasks;

namespace Rozetka.BLL.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetUserCartAsync(string userId);
        Task AddToCartAsync(string userId, int productId);
        Task RemoveItemAsync(int cartItemId);
        Task ClearCartAsync(string userId);
    }
}
