using System.Threading.Tasks;
using BLL.DTOs;

namespace BLL
{
    public interface ICartService
    {
        Task<CartReadDto> GetCartByUserIdAsync(string userId);

        Task AddToCartAsync(string userId, int productId);

        Task UpdateQuantityAsync(int cartItemId, int delta);

        Task RemoveItemAsync(int cartItemId);

        Task ClearCartAsync(string userId);
    }
}