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

        public IActionResult WhoAmI()
        {
            return Content(
                $"User: {User.Identity?.Name}\n" +
                $"Authenticated: {User.Identity?.IsAuthenticated}\n" +
                $"IsAdmin: {User.IsInRole("Admin")}");
        }
    }
}