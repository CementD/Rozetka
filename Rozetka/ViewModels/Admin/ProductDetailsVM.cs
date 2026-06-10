namespace Rozetka.ViewModels.Admin
{
    public class ProductDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string Specifications { get; set; }
        public string CategoryName { get; set; }

        public int OrdersCount { get; set; }
        public List<OrderMiniVM> Orders { get; set; }
    }
}
