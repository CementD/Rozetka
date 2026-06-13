using System.Collections.Generic;

namespace BLL.DTOs
{
    public class CartItemReadDto
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
    }
    public class CartReadDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public List<CartItemReadDto> Items { get; set; } = new();

        public decimal CartTotal { get; set; }
    }
}