using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Rozetka.ViewModels.Admin
{
    public class ProductVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва обов'язкова")]
        [StringLength(100, ErrorMessage = "Максимум 100 символів")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Опис обов'язковий")]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Ціна обов'язкова")]
        [Range(0.01, 1000000, ErrorMessage = "Некоректна ціна")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Зображення обов'язкове")]
        [Url(ErrorMessage = "Некоректний URL")]
        public string ImageUrl { get; set; }

        [StringLength(2000)]
        public string Specifications { get; set; }

        [Required(ErrorMessage = "Категорія обов'язкова")]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
