using Microsoft.AspNetCore.Mvc;
using BLL.Repositories;
using Domain;
using System.Threading.Tasks;

namespace Rozetka.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductRepository _repo;
        public AdminController(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _repo.GetAllAsync();
            return View(products);
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product model)
        {
            if (!ModelState.IsValid) return View(model);
            await _repo.AddAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(Product model)
        {
            if (!ModelState.IsValid) return View(model);
            await _repo.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
