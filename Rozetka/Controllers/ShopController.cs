using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rozetka.Controllers
{
    [Authorize]
    public class ShopController : Controller
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var shop = await _shopService.GetShopByOwnerIdAsync(userId);
            return View(shop);
        }

        public async Task<IActionResult> Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existing = await _shopService.GetShopByOwnerIdAsync(userId);

            if (existing != null)
                return RedirectToAction(nameof(Index));

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("name", "Назва магазину обов'язкова");
                return View();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _shopService.CreateShopRequestAsync(userId, name, description ?? "");

            if (!result)
            {
                ModelState.AddModelError("", "Заявка вже існує");
                return View();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
