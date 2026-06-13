using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public interface IFavoriteService
    {
        Task<bool> ToggleFavoriteAsync(string userId, int productId);

        Task<IEnumerable<Favorite>> GetUserFavoritesAsync(string userId);
    }
}