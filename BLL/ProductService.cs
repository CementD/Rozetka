using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DLL.Repositories;
using Domain;
using BLL.DTOs;

namespace BLL
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;

        public ProductService(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<IEnumerable<ProductReadDto>> GetAllProductsAsync()
        {
            var products = await _productRepo.GetAllAsync();
            return products.Select(p => new ProductReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description, 
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Specifications = p.Specifications,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? "Без категорії"
            });
        }

        public async Task<ProductReadDto?> GetProductByIdAsync(int id)
        {
            if (id <= 0) return null;

            var p = await _productRepo.GetByIdAsync(id);
            if (p == null) return null;

            return new ProductReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Specifications = p.Specifications,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? "Без категорії"
            };
        }

        public async Task CreateProductAsync(ProductCreateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.Price < 0) throw new InvalidOperationException("Ціна не може бути негативною.");

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                Specifications = dto.Specifications,
                CategoryId = dto.CategoryId
            };

            await _productRepo.AddAsync(product);
        }

        public async Task UpdateProductAsync(ProductUpdateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.Id <= 0) throw new ArgumentException("Некоректний ID.");

            var product = new Product
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                Specifications = dto.Specifications,
                CategoryId = dto.CategoryId
            };

            await _productRepo.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            if (id <= 0) return;
            await _productRepo.DeleteAsync(id);
        }

        public async Task<List<ProductReadDto>> GetProductsByCategoryAsync(int categoryId)
        {
            if (categoryId <= 0) return new List<ProductReadDto>();
            var products = await _productRepo.GetProductsByCategoryIdAsync(categoryId);

            return products.Select(p => new ProductReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Specifications = p.Specifications,
                CategoryId = p.CategoryId
            }).ToList();
        }

        public async Task<List<Order>> GetOrdersWithProductAsync(int productId)
        {
            if (productId <= 0) return new List<Order>();
            return await _productRepo.GetProductOrdersAsync(productId);
        }
    }
}