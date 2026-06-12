using Domain;
using Microsoft.AspNetCore.Mvc;
using Rozetka.BLL.Repositories;
using System.Threading.Tasks;

namespace Rozetka.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _repo;
        public ProductController(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _repo.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }
    }
}
