using Final_MozArt.Models;
using Final_MozArt.ViewModels.About;
using Final_MozArt.ViewModels.Support;

namespace Final_MozArt.Services.Interfaces
{
    public interface IAboutService
    {
        Task<List<AboutVM>> GetAllAsync();
        Task<AboutVM> GetByIdAsync(int id);
        Task<About> GetEntityByIdAsync(int id);
        Task CreateAsync(AboutCreateVM request);
        Task DeleteAsync(int id);
        Task EditAsync(AboutEditVM request);
    }
}
