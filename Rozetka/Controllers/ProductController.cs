using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BLL;

namespace Rozetka.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var productsResult = await _productService.GetAllProductsAsync();

            var productsList = productsResult.ToList();

            var categories = productsList
                .Where(p => !string.IsNullOrEmpty(p.CategoryName))
                .Select(p => p.CategoryName)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            ViewData["Categories"] = categories;

            return View(productsList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null) return NotFound();

            return View(product);
        }
    }
}