using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Rozetka.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly IFavoriteService _favoriteService;

        public FavoritesController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Challenge();

            var favorites = await _favoriteService.GetUserFavoritesAsync(userId);
            return View(favorites);
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Challenge();

            await _favoriteService.ToggleFavoriteAsync(userId, id);

            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
                return Redirect(referer);

            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleJson(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Json(new { success = false });

            await _favoriteService.ToggleFavoriteAsync(userId, id);

            var favs = await _favoriteService.GetUserFavoritesAsync(userId);
            bool isFav = favs.Any(f => f.ProductId == id);

            return Json(new { success = true, isFavorite = isFav });
        }
    }
}
