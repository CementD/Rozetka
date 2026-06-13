namespace BLL.DTOs
{
    public class CategoryCreateDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class CategoryUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class CategoryReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ProductReadDto> Products { get; set; } = new();
    }
}