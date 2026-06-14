using BLL.DTOs;
using System.Collections.Generic;

namespace Rozetka.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<ProductReadDto> Products { get; set; }
        public IEnumerable<CategoryReadDto> Categories { get; set; }
        public IEnumerable<ShopDto> Stores { get; set; }
    }
}
