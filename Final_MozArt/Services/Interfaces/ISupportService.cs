using Final_MozArt.Models;
using Final_MozArt.ViewModels.Slider;
using Final_MozArt.ViewModels.Support;

namespace Final_MozArt.Services.Interfaces
{
    public interface ISupportService
    {
        Task<List<SupportVM>> GetAllAsync();
        Task<SupportVM> GetByIdAsync(int id);
        Task<Support> GetEntityByIdAsync(int id);
        Task CreateAsync(SupportCreateVM request);
        Task DeleteAsync(int id);
        Task EditAsync(SupportEditVM request);
    }
}
