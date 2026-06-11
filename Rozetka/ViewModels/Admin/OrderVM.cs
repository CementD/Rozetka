namespace Rozetka.ViewModels.Admin
{
    public class OrderVM
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public decimal Total { get; set; }
        public int OrderItemsCount { get; set; }
    }
}