using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using BLL;

namespace Rozetka.Controllers
{
    [Authorize(Roles = "Seller")]
    public class SellerController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public SellerController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }



        public async Task<IActionResult> AddProduct()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var model = new ViewModels.Admin.ProductCreateVM
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(ViewModels.Admin.ProductCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                model.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

                return View(model);
            }

            var dto = new BLL.DTOs.ProductCreateDto
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                ImageUrl = model.ImageUrl,
                Specifications = model.Specifications,
                CategoryId = model.CategoryId ?? 0
            };

            await _productService.CreateProductAsync(dto);

            return RedirectToAction("Index", "Home");
        }
    }
}
