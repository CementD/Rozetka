using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DLL.Repositories;
using Domain;
using BLL.DTOs;

namespace BLL
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryService(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepo.GetAllAsync();

            return categories.Select(c => new CategoryReadDto
            {
                Id = c.Id,
                Name = c.Name
            });
        }

        public async Task<CategoryReadDto?> GetCategoryByIdAsync(int id)
        {
            if (id <= 0) return null;

            var c = await _categoryRepo.GetByIdAsync(id);
            if (c == null) return null;

            return new CategoryReadDto
            {
                Id = c.Id,
                Name = c.Name,
                Products = c.Products?.Select(p => new ProductReadDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Specifications = p.Specifications,
                    CategoryId = p.CategoryId
                }).ToList() ?? new List<ProductReadDto>()
            };
        }
        public async Task CreateCategoryAsync(CategoryCreateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new InvalidOperationException("Назва категорії не може бути порожньою.");
            }

            var category = new Category
            {
                Name = dto.Name
            };

            await _categoryRepo.AddAsync(category);
            await _categoryRepo.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(CategoryUpdateDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.Id <= 0) throw new ArgumentException("Некоректний ID категорії.");
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new InvalidOperationException("Назва не може бути порожньою.");

            var category = new Category
            {
                Id = dto.Id,
                Name = dto.Name
            };

            _categoryRepo.Update(category);
            await _categoryRepo.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            if (id <= 0) return;

            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null) return;

            _categoryRepo.Delete(category);
            await _categoryRepo.SaveChangesAsync();
        }
    }
}