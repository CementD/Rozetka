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

        public async Task<IActionResult> Index(string? search, string? category, string? sort)
        {
            var productsResult = await _productService.GetAllProductsAsync();
            var products = productsResult.ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();
                products = products.Where(p =>
                    p.Name.Contains(s, StringComparison.OrdinalIgnoreCase) ||
                    (!string.IsNullOrEmpty(p.Description) && p.Description.Contains(s, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                products = products.Where(p =>
                    p.CategoryName.Equals(category, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            products = sort switch
            {
                "price_asc" => products.OrderBy(p => p.Price).ToList(),
                "price_desc" => products.OrderByDescending(p => p.Price).ToList(),
                "name" => products.OrderBy(p => p.Name).ToList(),
                _ => products
            };

            var allProducts = await _productService.GetAllProductsAsync();
            var categories = allProducts
                .Where(p => !string.IsNullOrEmpty(p.CategoryName))
                .Select(p => p.CategoryName)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            ViewData["Categories"] = categories;
            ViewData["Search"] = search;
            ViewData["Category"] = category;
            ViewData["Sort"] = sort;

            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }
    }
}
