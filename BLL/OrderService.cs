using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DLL.Repositories;
using Domain;
using BLL.DTOs;

namespace BLL
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;

        public OrderService(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<IEnumerable<OrderReadDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepo.GetAllAsync();

            return orders.Select(o => new OrderReadDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status,
                UserId = o.UserId,
                UserName = o.User?.UserName ?? "Гість",
                Total = o.OrderItems?.Sum(i => i.UnitPrice * i.Quantity) ?? 0m,
                OrderItemsCount = o.OrderItems?.Count ?? 0
            });
        }

        public async Task<OrderDetailsDto?> GetOrderByIdAsync(int id)
        {
            if (id <= 0) return null;

            var order = await _orderRepo.GetByIdWithDetailsAsync(id);
            if (order == null) return null;

            return new OrderDetailsDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                UserId = order.UserId,
                UserName = order.User?.UserName ?? "Невідомий користувач",
                Items = order.OrderItems.Select(i => new OrderItemReadDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? "Товар видалено",
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };
        }

        public async Task<IEnumerable<OrderReadDto>> GetOrdersByUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return Enumerable.Empty<OrderReadDto>();

            var orders = await _orderRepo.GetByUserIdAsync(userId);

            return orders.Select(o => new OrderReadDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status,
                UserId = o.UserId,
                Total = o.OrderItems.Sum(i => i.UnitPrice * i.Quantity),
                OrderItemsCount = o.OrderItems.Count
            });
        }

        public async Task<OrderDetailsDto?> CreateOrderFromCartAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return null;

            var newOrder = await _orderRepo.AddOrderFromCartAsync(userId);
            if (newOrder == null) return null;

            return new OrderDetailsDto
            {
                Id = newOrder.Id,
                OrderDate = newOrder.OrderDate,
                Status = newOrder.Status,
                UserId = newOrder.UserId,
                Items = newOrder.OrderItems.Select(i => new OrderItemReadDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };
        }

        public async Task UpdateOrderStatusAsync(OrderUpdateStatusDto dto)
        {
            if (dto == null || dto.Id <= 0) return;

            var order = await _orderRepo.GetByIdAsync(dto.Id);
            if (order == null) return;

            order.Status = dto.Status;

            await _orderRepo.UpdateAsync(order);
            await _orderRepo.SaveChangesAsync();
        }

        public async Task CancelOrderAsync(int id)
        {
            if (id <= 0) return;
            await _orderRepo.CancelOrderAsync(id);
        }
    }
}