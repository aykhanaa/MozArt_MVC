using Final_MozArt.ViewModels.Category;

namespace Final_MozArt.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryVM>> GetAllAsync();
        Task<CategoryVM> GetByIdAsync(int id);
        Task CreateAsync(CategoryCreateVM request);
        Task DeleteAsync(int id);
        Task EditAsync(CategoryEditVM request);
        Task<Dictionary<string, int>> GetProductCountByCategoryNameAsync();
    }
}
