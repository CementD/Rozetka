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
        private readonly IShopService _shopService;

        public AdminController(
            IProductService productService,
            ICategoryService categoryService,
            IUserService userService,
            IOrderService orderService,
            IShopService shopService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _userService = userService;
            _orderService = orderService;
            _shopService = shopService;
        }

        public async Task<IActionResult> Index(string tab = "products")
        {
            var products = await _productService.GetAllProductsAsync();
            var categories = await _categoryService.GetAllCategoriesAsync();
            var users = await _userService.GetAllUsersAsync();
            var orders = await _orderService.GetAllOrdersAsync();
            var shops = await _shopService.GetAllShopsAsync();
            var pendingShops = await _shopService.GetPendingShopsAsync();

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

                Shops = shops.ToList(),

                PendingShops = pendingShops.ToList(),

                ProductsCount = products.Count(),
                CategoriesCount = categories.Count(),
                OrdersCount = orders.Count(),
                UsersCount = users.Count(),
                ShopsCount = shops.Count(),
                PendingCount = pendingShops.Count()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveShop(int id)
        {
            await _shopService.ApproveShopAsync(id);
            return RedirectToAction(nameof(Index), new { tab = "shops" });
        }

        public async Task<IActionResult> AddProduct()
        {
            await LoadCategories();
            return View(new ProductCreateVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(ProductCreateVM model)
        {
            if (!ModelState.IsValid) { await LoadCategories(); return View(model); }
            await _productService.CreateProductAsync(new ProductCreateDto
            {
                Name = model.Name,
                Description = model.Description ?? "",
                Price = model.Price,
                ImageUrl = model.ImageUrl ?? "",
                Specifications = model.Specifications,
                CategoryId = (int)model.CategoryId
            });
            return RedirectToAction(nameof(Index), new { tab = "products" });
        }

        public async Task<IActionResult> EditProduct(int id)
        {
            var p = await _productService.GetProductByIdAsync(id);
            if (p == null) return NotFound();
            await LoadCategories(p.CategoryId);
            return View(new ProductEditVM
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Specifications = p.Specifications,
                CategoryId = p.CategoryId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, ProductEditVM model)
        {
            if (!ModelState.IsValid) { await LoadCategories(model.CategoryId); return View(model); }
            await _productService.UpdateProductAsync(new ProductUpdateDto
            {
                Id = id,
                Name = model.Name,
                Description = model.Description ?? "",
                Price = model.Price,
                ImageUrl = model.ImageUrl ?? "",
                Specifications = model.Specifications,
                CategoryId = (int)model.CategoryId
            });
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
            var p = await _productService.GetProductByIdAsync(id);
            if (p == null) return NotFound();
            var orders = await _productService.GetOrdersWithProductAsync(id);
            var vm = new ProductDetailsVM
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Specifications = p.Specifications,
                CategoryId = p.CategoryId,
                CategoryName = p.CategoryName,
                ShopId = p.ShopId,
                ShopName = p.ShopName,
                OrdersCount = orders.Count,
                Orders = orders.Select(o => new OrderMiniVM
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status
                }).ToList()
            };
            return View(vm);
        }

        public IActionResult AddCategory() => View(new CategoryVM());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(CategoryVM model)
        {
            if (!ModelState.IsValid) return View(model);
            await _categoryService.CreateCategoryAsync(new CategoryCreateDto { Name = model.Name });
            return RedirectToAction(nameof(Index), new { tab = "categories" });
        }

        public async Task<IActionResult> EditCategory(int id)
        {
            var c = await _categoryService.GetCategoryByIdAsync(id);
            if (c == null) return NotFound();
            return View(new CategoryVM { Id = c.Id, Name = c.Name });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(CategoryVM model)
        {
            if (!ModelState.IsValid) return View(model);
            await _categoryService.UpdateCategoryAsync(new CategoryUpdateDto { Id = model.Id, Name = model.Name });
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
            var c = await _categoryService.GetCategoryByIdAsync(id);
            if (c == null) return NotFound();
            var products = await _productService.GetProductsByCategoryAsync(id);
            return View(new CategoryDetailsVM
            {
                Id = c.Id,
                Name = c.Name,
                Products = products.Select(p => new ProductVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl
                }).ToList()
            });
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            var o = await _orderService.GetOrderByIdAsync(id);
            if (o == null) return NotFound();
            return View(new OrderDetailsVM
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status,
                UserId = o.UserId,
                UserName = o.UserName,
                Items = o.Items.Select(i => new OrderItemVM
                {
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            });
        }

        public async Task<IActionResult> UserDetails(string id)
        {
            var u = await _userService.GetUserByIdAsync(id);
            if (u == null) return NotFound();
            return View(new UserDetailsVM
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Address = u.Address,
                Orders = u.Orders.Select(o => new OrderMiniVM
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    Total = o.Total
                }).ToList()
            });
        }

        private async Task LoadCategories(int? selectedId = null)
        {
            var cats = await _categoryService.GetAllCategoriesAsync();
            ViewBag.Categories = new SelectList(cats, "Id", "Name", selectedId);
        }

        private async Task<IActionResult> ShopDetails(int id)
        {
            var shopDetails = await _shopService.GetShopDetailsByIdAsync(id);

            if (shopDetails == null) return NotFound();

            return View(shopDetails);
        }
    }
}