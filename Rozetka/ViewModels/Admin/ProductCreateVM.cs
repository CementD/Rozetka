using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Rozetka.ViewModels.Admin
{
    public class ProductCreateVM
    {
        [Required]
        public string Name { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public string ImageUrl { get; set; } = "";

        public string? Specifications { get; set; }

        [Required(ErrorMessage = "Оберіть категорію")]
        public int? CategoryId { get; set; }

        [ValidateNever]
        public List<SelectListItem> Categories { get; set; } = new();
    }
}