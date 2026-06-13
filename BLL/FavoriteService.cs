using Domain;
using DLL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoriteService(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<bool> ToggleFavoriteAsync(string userId, int productId)
        {
            var isFavorite = await _favoriteRepository.IsFavoriteAsync(userId, productId);

            if (isFavorite)
            {
                await _favoriteRepository.RemoveAsync(userId, productId);
                await _favoriteRepository.SaveChangesAsync();
                return false; 
            }
            else
            {
                var favorite = new Favorite
                {
                    UserId = userId,
                    ProductId = productId
                };

                await _favoriteRepository.AddAsync(favorite);
                await _favoriteRepository.SaveChangesAsync();
                return true;
            }
        }

        public async Task<IEnumerable<Favorite>> GetUserFavoritesAsync(string userId)
        {
            return await _favoriteRepository.GetByUserIdAsync(userId);
        }
    }
}