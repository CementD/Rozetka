using Domain;

namespace Rozetka.ViewModels.Admin
{
    public class OrderDetailsVM
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }

        public List<OrderItemVM> Items { get; set; }
    }
}
