using Microsoft.AspNetCore.Mvc;

namespace Rozetka.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
