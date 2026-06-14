using BLL.DTOs;

namespace BLL
{
    public interface IShopService
    {
        Task<ShopDto?> GetShopByOwnerIdAsync(string ownerId);
        Task<IEnumerable<ShopDto>> GetPendingShopsAsync();
        Task<bool> CreateShopRequestAsync(string userId, string name, string description);
        Task<bool> ApproveShopAsync(int shopId);
    }
}