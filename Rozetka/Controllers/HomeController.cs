using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Rozetka.Models;
using Rozetka.BLL.Repositories;
using System.Threading.Tasks;
using ViewModels;


namespace Rozetka.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepo, ICategoryRepository categoryRepo)
        {
            _logger = logger;
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepo.GetAllAsync();
            var categories = await _categoryRepo.GetAllAsync();
            var vm = new HomeViewModel
            {
                Products = products,
                Categories = categories
            };
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
