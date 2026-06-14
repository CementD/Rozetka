using Domain;

namespace DLL.Repositories
{
    public interface IShopRepository
    {
        Task<Shop?> GetByIdAsync(int id);
        Task<Shop?> GetByOwnerIdAsync(string ownerId);
        Task<IEnumerable<Shop>> GetPendingShopsAsync();
        Task AddAsync(Shop shop);
        void Update(Shop shop);
        Task SaveChangesAsync();
        Task<IEnumerable<Shop>> GetAllShopsAsync();
    }
}