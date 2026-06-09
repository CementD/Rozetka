using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Rozetka.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Ноутбук Lenovo IdeaPad",
                    Description = "15.6 Full HD / Ryzen 5 / 16GB RAM",
                    Price = 24999,
                    ImageUrl = "/images/laptop.jpg"
                },
                new Product
                {
                    Id = 2,
                    Name = "Смартфон Samsung Galaxy",
                    Description = "128GB / AMOLED / NFC",
                    Price = 16499,
                    ImageUrl = "/images/phone.jpg"
                }
            };

            return View(products);
        }
    }
}
