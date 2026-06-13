using System;
using System.Collections.Generic;

namespace BLL.DTOs
{
    public class OrderItemReadDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
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
    }

    public class OrderUpdateStatusDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}