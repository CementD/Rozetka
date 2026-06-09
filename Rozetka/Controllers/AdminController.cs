using Microsoft.AspNetCore.Mvc;

namespace Rozetka.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
