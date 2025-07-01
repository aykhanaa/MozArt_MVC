using Final_MozArt.Models;
using Final_MozArt.ViewModels.Slider;

namespace Final_MozArt.Services.Interfaces
{
    public interface ISliderService
    {
        Task<List<SliderVM>> GetAllAsync();
        Task<SliderVM> GetByIdAsync(int id);
        Task<Slider> GetEntityByIdAsync(int id); 
        Task CreateAsync(SliderCreateVM request);
        Task DeleteAsync(int id);
        Task EditAsync(SliderEditVM request);
    }
}
