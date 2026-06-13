using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DLL.Repositories;
using BLL.DTOs;

namespace BLL
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;

        public CartService(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }

        public async Task<CartReadDto> GetCartByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return new CartReadDto();

            var cart = await _cartRepo.GetUserCartAsync(userId);

            var cartDto = new CartReadDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = cart.Items?.Select(i => new CartItemReadDto
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? "Товар видалено",
                    ProductImageUrl = i.Product?.ImageUrl ?? string.Empty,
                    Description = i.Product?.Description ?? string.Empty,
                    UnitPrice = i.Product?.Price ?? 0m,
                    Quantity = i.Quantity
                }).ToList() ?? new List<CartItemReadDto>()
            };

            cartDto.CartTotal = cartDto.Items.Sum(item => item.TotalPrice);

            return cartDto;
        }

        public async Task AddToCartAsync(string userId, int productId)
        {
            if (string.IsNullOrEmpty(userId) || productId <= 0) return;

            await _cartRepo.AddToCartAsync(userId, productId);
        }

        public async Task UpdateQuantityAsync(int cartItemId, int delta)
        {
            if (cartItemId <= 0 || delta == 0) return;

            await _cartRepo.UpdateQuantityAsync(cartItemId, delta);
        }

        public async Task RemoveItemAsync(int cartItemId)
        {
            if (cartItemId <= 0) return;

            await _cartRepo.RemoveItemAsync(cartItemId);
        }

        public async Task ClearCartAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return;

            await _cartRepo.ClearCartAsync(userId);
        }
    }
}