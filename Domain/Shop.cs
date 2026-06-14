namespace Domain
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public string OwnerId { get; set; } = string.Empty;
        public User Owner { get; set; }

        public bool IsApproved { get; set; } = false;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}