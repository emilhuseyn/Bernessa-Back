using App.Business.DTOs.Categories;

namespace App.Business.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO> GetCategoryByIdAsync(int id);
        Task<CategoryDTO> GetCategoryBySlugAsync(string slug);
        Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO createCategoryDto);
        Task<CategoryDTO> UpdateCategoryAsync(int id, CreateCategoryDTO updateCategoryDto);
        Task DeleteCategoryAsync(int id);
    }
}
