using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Rozetka.Controllers
{
    [Authorize]
    public class ShopController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string storeName)
        {
            if (string.IsNullOrWhiteSpace(storeName))
            {
                ModelState.AddModelError("", "Store name is required");
                return View();
            }
            await Task.CompletedTask;
            return RedirectToAction("Index", "Home");
        }
    }
}
