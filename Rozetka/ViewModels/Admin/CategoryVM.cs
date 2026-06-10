using System.ComponentModel.DataAnnotations;

namespace Rozetka.ViewModels.Admin
{
    public class CategoryVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва категорії обов'язкова")]
        [StringLength(100, ErrorMessage = "Максимум 100 символів")]
        public string Name { get; set; }
    }
}
