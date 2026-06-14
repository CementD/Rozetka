namespace BLL.DTOs
{
    public class ShopDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string OwnerId { get; set; } = string.Empty;
        public bool IsApproved { get; set; }

        public string OwnerEmail { get; set; } = string.Empty;
        public string OwnerFullName { get; set; } = string.Empty;
    }
}