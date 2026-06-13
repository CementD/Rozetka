using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DTOs;

namespace BLL
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync();
        Task<CategoryReadDto?> GetCategoryByIdAsync(int id);
        Task CreateCategoryAsync(CategoryCreateDto dto);
        Task UpdateCategoryAsync(CategoryUpdateDto dto);
        Task DeleteCategoryAsync(int id);
    }
}