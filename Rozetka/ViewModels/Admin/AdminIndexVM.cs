namespace Rozetka.ViewModels.Admin
{
    public class AdminIndexVM
    {
        public string CurrentTab { get; set; }

        public List<ProductVM> Products { get; set; } = new();
        public List<CategoryVM> Categories { get; set; } = new();

        public int ProductsCount { get; set; }
        public int CategoriesCount { get; set; }
    }
}
