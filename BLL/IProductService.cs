using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DTOs;
using Domain;

namespace BLL
{
    public interface IProductService
    {
        Task<IEnumerable<ProductReadDto>> GetAllProductsAsync();

        Task<ProductReadDto?> GetProductByIdAsync(int id);

        Task CreateProductAsync(ProductCreateDto dto, int? shopId = null);
        Task UpdateProductAsync(ProductUpdateDto dto);

        Task DeleteProductAsync(int id);

        Task<List<ProductReadDto>> GetProductsByCategoryAsync(int categoryId);

        Task<List<Order>> GetOrdersWithProductAsync(int productId);
    }
}