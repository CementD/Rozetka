namespace Rozetka.ViewModels.Admin
{
    public class OrderMiniVM
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Status { get; set; }
        public decimal Total { get; set; }
    }
}
