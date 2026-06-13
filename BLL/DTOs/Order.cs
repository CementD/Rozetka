using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DTOs
{
    public class OrderItemReadDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public List<OrderItemReadDto> Items { get; set; } = new();
        public decimal Total => Items.Sum(i => i.UnitPrice * i.Quantity);
    }

    public class OrderReadDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public int OrderItemsCount { get; set; }
        public List<OrderItemReadDto> Items { get; set; } = new();
    }

    public class OrderUpdateStatusDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}