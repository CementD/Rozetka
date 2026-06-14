using BLL.DTOs;
using DLL.Repositories;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace BLL
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository _shopRepository;
        private readonly UserManager<User> _userManager;

        public ShopService(IShopRepository shopRepository, UserManager<User> userManager)
        {
            _shopRepository = shopRepository;
            _userManager = userManager;
        }

        public async Task<ShopDto?> GetShopByOwnerIdAsync(string ownerId)
        {
            var shop = await _shopRepository.GetByOwnerIdAsync(ownerId);
            if (shop == null) return null;

            return new ShopDto
            {
                Id = shop.Id,
                Name = shop.Name,
                Description = shop.Description,
                OwnerId = shop.OwnerId,
                IsApproved = shop.IsApproved
            };
        }

        public async Task<IEnumerable<ShopDto>> GetPendingShopsAsync()
        {
            var shops = await _shopRepository.GetPendingShopsAsync();

            return shops.Select(s => new ShopDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                OwnerId = s.OwnerId,
                IsApproved = s.IsApproved,
                OwnerEmail = s.Owner?.Email ?? "???",
                OwnerFullName = s.Owner != null ? $"{s.Owner.FirstName} {s.Owner.LastName}" : "Невідомо"
            });
        }

        public async Task<bool> CreateShopRequestAsync(string userId, string name, string description)
        {
            var existingShop = await _shopRepository.GetByOwnerIdAsync(userId);
            if (existingShop != null) return false;

            var newShop = new Shop
            {
                Name = name,
                Description = description,
                OwnerId = userId,
                IsApproved = false
            };

            await _shopRepository.AddAsync(newShop);
            await _shopRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveShopAsync(int shopId)
        {
            var shop = await _shopRepository.GetByIdAsync(shopId);
            if (shop == null) return false;

            shop.IsApproved = true;
            _shopRepository.Update(shop);
            await _shopRepository.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(shop.OwnerId);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, "Seller");
            }

            return true;
        }
    }
}