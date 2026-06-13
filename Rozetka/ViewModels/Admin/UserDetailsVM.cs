namespace Rozetka.ViewModels.Admin
{
    public class UserDetailsVM
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public List<OrderMiniVM> Orders { get; set; }
    }
}
