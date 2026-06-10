using BLL.Repositories;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rozetka.BLL.Repositories;
using Rozetka.ViewModels.Admin;

namespace Rozetka.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IUserRepository _userRepo;
        private readonly IOrderRepository _orderRepo;

        public AdminController(
            IProductRepository productRepo,
            ICategoryRepository categoryRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index(string tab = "products")
        {
            var products = await _productRepo.GetAllAsync();
            var categories = await _categoryRepo.GetAllAsync();

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
                    CategoryName = p.Category?.Name
                }).ToList(),

                Categories = categories.Select(c => new CategoryVM
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList(),

                ProductsCount = products.Count(),
                CategoriesCount = categories.Count()
            };

            return View(model);
        }


        public async Task<IActionResult> AddProduct()
        {
            var categories = await _categoryRepo.GetAllAsync();

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
                var categories = await _categoryRepo.GetAllAsync();

                model.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

                return View(model);
            }

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                ImageUrl = model.ImageUrl,
                Specifications = model.Specifications,
                CategoryId = (int)model.CategoryId
            };

            await _productRepo.AddAsync(product);

            return RedirectToAction(nameof(Index), new { tab = "products" });
        }

        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            var categories = await _categoryRepo.GetAllAsync();

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
                var categories = await _categoryRepo.GetAllAsync();

                model.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

                return View(model);
            }

            var product = await _productRepo.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.ImageUrl = model.ImageUrl;
            product.Specifications = model.Specifications;
            product.CategoryId = (int)model.CategoryId;

            await _productRepo.UpdateAsync(product);

            return RedirectToAction(nameof(Index), new { tab = "products" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productRepo.DeleteAsync(id);

            return RedirectToAction(nameof(Index), new { tab = "products" });
        }

        public async Task<IActionResult> ProductDetails(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            var orders = await _productRepo.GetProductOrdersAsync(id);

            var model = new ProductDetailsVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Specifications = product.Specifications,
                CategoryName = product.Category?.Name,

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

            var category = new Category
            {
                Name = model.Name
            };

            await _categoryRepo.AddAsync(category);
            await _categoryRepo.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { tab = "categories" });
        }

        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);

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

            var category = await _categoryRepo.GetByIdAsync(model.Id);

            if (category == null)
                return NotFound();

            category.Name = model.Name;

            _categoryRepo.Update(category);
            await _categoryRepo.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { tab = "categories" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            _categoryRepo.Delete(category);
            await _categoryRepo.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { tab = "categories" });
        }
        public async Task<IActionResult> CategoryDetails(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            var products = await _productRepo.GetProductsByCategoryIdAsync(id);

            var model = new CategoryDetailsVM
            {
                Id = category.Id,
                Name = category.Name,

                Products = products.Select(p => new ProductVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl
                }).ToList()
            };

            return View(model);
        }

        public IActionResult WhoAmI()
        {
            return Content(
                $"User: {User.Identity?.Name}\n" +
                $"Authenticated: {User.Identity?.IsAuthenticated}\n" +
                $"IsAdmin: {User.IsInRole("Admin")}");
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = _orderRepo.GetByIdWithDetailsAsync(id).Result;

            if (order == null)
                return NotFound();

            var model = new OrderDetailsVM
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                UserId = order.UserId,
                UserName = order.User.UserName,

                Items = order.OrderItems.Select(i => new OrderItemVM
                {
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            return View(model);
        }

        public async Task<IActionResult> UserDetails(string id)
        {
            var user = await _userRepo.GetByIdWithOrdersAsync(id);

            if (user == null)
                return NotFound();

            var model = new UserDetailsVM
            {
                Id = user.Id,
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}",

                Orders = user.Orders.Select(o => new OrderMiniVM
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status
                }).ToList()
            };

            return View(model);
        }
    }
}