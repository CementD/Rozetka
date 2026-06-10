namespace Rozetka.ViewModels.Admin
{
    public class CategoryDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ProductVM> Products { get; set; }
    }
}
