using Final_MozArt.Models;
using Final_MozArt.ViewModels.Color;

namespace Final_MozArt.Services.Interfaces
{
    public interface IColorService
    {
        Task<List<ColorVM>> GetAllAsync();
        Task<ColorVM> GetByIdAsync(int id);
        Task<Color> GetEntityByIdAsync(int id);
        Task CreateAsync(ColorCreateVM request);
        Task DeleteAsync(int id);
        Task EditAsync(ColorEditVM request);
    }
}
