using Final_MozArt.Models;
using Final_MozArt.ViewModels.BlogCategory;

namespace Final_MozArt.Services.Interfaces
{
    public interface IBlogCategoryService
    {
        Task<List<BlogCategoryVM>> GetAllAsync();
        Task<BlogCategoryVM> GetByIdAsync(int id);
        Task<BlogCategory> GetEntityByIdAsync(int id);
        Task CreateAsync(BlogCategoryCreateVM request);
        Task DeleteAsync(int id);
        Task EditAsync(BlogCategoryEditVM request);
    }
}
