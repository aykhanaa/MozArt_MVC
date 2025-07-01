using Final_MozArt.ViewModels.Brand;
using Final_MozArt.ViewModels.Category;

namespace Final_MozArt.Services.Interfaces
{
    public interface IBrandService
    {
        Task<List<BrandVM>> GetAllAsync();
        Task<BrandVM> GetByIdAsync(int id);
        Task CreateAsync(BrandCreateVM request);
        Task DeleteAsync(int id);
        Task EditAsync(BrandEditVM request);
        Task<Dictionary<string, int>> GetProductCountByBrandNameAsync();

    }
}
