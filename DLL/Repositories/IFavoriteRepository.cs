using Domain;

namespace DLL.Repositories
{
    public interface IFavoriteRepository
    {
        Task AddAsync(Favorite favorite);

        Task RemoveAsync(string userId, int productId);

        Task<IEnumerable<Favorite>> GetByUserIdAsync(string userId);

        Task<bool> IsFavoriteAsync(string userId, int productId);

        Task<bool> SaveChangesAsync();
    }
}
