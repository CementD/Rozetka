using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public new string? PhoneNumber { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
