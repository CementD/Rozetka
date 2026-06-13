using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL;

namespace Rozetka.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public OrderController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();

            var orders = await _orderService.GetOrdersByUserAsync(userId);

            return View(orders.ToList());
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create()
        {
            return RedirectToAction("Checkout");
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var userId = GetUserId();
            var user = await _userService.GetUserByIdAsync(userId);

            var vm = new ViewModels.CheckoutVM
            {
                CardNumber = user?.CardNumber,
                CardExpiry = user?.CardExpiry,
                CardCvv = user?.CardCvv
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(ViewModels.CheckoutVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = GetUserId();

            await _userService.UpdatePaymentInfoAsync(userId, model.CardNumber, model.CardExpiry, model.CardCvv);
            var order = await _orderService.CreateOrderFromCartAsync(userId);

            if (order == null) return RedirectToAction("Index", "Cart");

            return RedirectToAction("Details", new { id = order.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            await _orderService.CancelOrderAsync(id);
            return RedirectToAction("Index");
        }
    }
}