using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BLL;
using BLL.DTOs;
using Rozetka.ViewModels.Admin;

namespace Rozetka.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

        public AdminController(
            IProductService productService,
            ICategoryService categoryService,
            IUserService userService,
            IOrderService orderService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _userService = userService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index(string tab = "products")
        {
            var products = await _productService.GetAllProductsAsync();
            var categories = await _categoryService.GetAllCategoriesAsync();
            var users = await _userService.GetAllUsersAsync();
            var orders = await _orderService.GetAllOrdersAsync();

            var model = new AdminIndexVM
            {
                CurrentTab = tab,

                Products = products.Select(p => new ProductVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Specifications = p.Specifications,
                    CategoryId = p.CategoryId,
                    CategoryName = p.CategoryName
                }).ToList(),

                Categories = categories.Select(c => new CategoryVM
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList(),

                Orders = orders.Select(o => new OrderVM
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    UserId = o.UserId,
                    UserName = o.UserName,
                    Total = o.Total,
                    OrderItemsCount = o.OrderItemsCount
                }).ToList(),

                Users = users.Select(u => new UserVM
                {
                    Id = u.Id,
                    Email = u.Email,
                    FullName = u.FullName,
                    PhoneNumber = u.PhoneNumber,
                    OrdersCount = u.OrdersCount
                }).ToList(),

                ProductsCount = products.Count(),
                CategoriesCount = categories.Count(),
                OrdersCount = orders.Count(),
                UsersCount = users.Count(),
            };

            return View(model);
        }

        public async Task<IActionResult> AddProduct()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            var model = new ProductCreateVM
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(ProductCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                model.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

                return View(model);
            }

            var dto = new ProductCreateDto
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                ImageUrl = model.ImageUrl,
                Specifications = model.Specifications,
                CategoryId = model.CategoryId ?? 0
            };

            await _productService.CreateProductAsync(dto);

            return RedirectToAction(nameof(Index), new { tab = "products" });
        }

        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();

            var categories = await _categoryService.GetAllCategoriesAsync();

            var model = new ProductEditVM
            {
                Id = id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Specifications = product.Specifications,
                CategoryId = product.CategoryId,

                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, ProductEditVM model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                model.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

                return View(model);
            }

            var dto = new ProductUpdateDto
            {
                Id = id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                ImageUrl = model.ImageUrl,
                Specifications = model.Specifications,
                CategoryId = model.CategoryId ?? 0
            };

            await _productService.UpdateProductAsync(dto);

            return RedirectToAction(nameof(Index), new { tab = "products" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index), new { tab = "products" });
        }

        public async Task<IActionResult> ProductDetails(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();

            var orders = await _productService.GetOrdersWithProductAsync(id);

            var model = new ProductDetailsVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Specifications = product.Specifications,
                CategoryName = product.CategoryName,

                OrdersCount = orders.Count,
                Orders = orders.Select(o => new OrderMiniVM
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status
                }).ToList()
            };

            return View(model);
        }

        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(CategoryVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new CategoryCreateDto
            {
                Name = model.Name
            };

            await _categoryService.CreateCategoryAsync(dto);

            return RedirectToAction(nameof(Index), new { tab = "categories" });
        }

        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
                return NotFound();

            return View(new CategoryVM
            {
                Id = category.Id,
                Name = category.Name
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(CategoryVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new CategoryUpdateDto
            {
                Id = model.Id,
                Name = model.Name
            };

            await _categoryService.UpdateCategoryAsync(dto);

            return RedirectToAction(nameof(Index), new { tab = "categories" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(Index), new { tab = "categories" });
        }

        public async Task<IActionResult> CategoryDetails(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
                return NotFound();

            var model = new CategoryDetailsVM
            {
                Id = category.Id,
                Name = category.Name,

                Products = category.Products.Select(p => new ProductVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl
                }).ToList()
            };

            return View(model);
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
                return NotFound();

            var model = new OrderDetailsVM
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                UserId = order.UserId,
                UserName = order.UserName,

                Items = order.Items.Select(i => new OrderItemVM
                {
                    ProductName = i.ProductName,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string status)
        {
            if (orderId <= 0 || string.IsNullOrEmpty(status))
            {
                return BadRequest("Некоректні дані замовлення");
            }

            var updateDto = new OrderUpdateStatusDto
            {
                Id = orderId,
                Status = status
            };

            await _orderService.UpdateOrderStatusAsync(updateDto);

            return RedirectToAction(nameof(OrderDetails), new { id = orderId });
        }

        public async Task<IActionResult> UserDetails(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            var model = new UserDetailsVM
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,

                Orders = user.Orders.Select(o => new OrderMiniVM
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    Total = o.Total
                }).ToList()
            };

            return View(model);
        }

        public async Task<IActionResult> PendingShops()
        {
            await Task.CompletedTask;
            var empty = new System.Collections.Generic.List<string>();
            return View(empty);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveShop(int id)
        {
            if (id <= 0)
                return BadRequest();

            await Task.CompletedTask;

            return RedirectToAction(nameof(PendingShops));
        }

        public async Task<IActionResult> ExistingShops()
        {
            await Task.CompletedTask;
            var empty = new System.Collections.Generic.List<string>();
            return View(empty);
        }

        public IActionResult WhoAmI()
        {
            return Content(
                $"User: {User.Identity?.Name}\n" +
                $"Authenticated: {User.Identity?.IsAuthenticated}\n" +
                $"IsAdmin: {User.IsInRole("Admin")}");
        }
    }
}