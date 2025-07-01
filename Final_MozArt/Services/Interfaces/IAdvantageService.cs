using Final_MozArt.Models;
using Final_MozArt.ViewModels.Advantage;


namespace Final_MozArt.Services.Interfaces
{
    public interface IAdvantageService
    {
        Task<List<AdvantageVM>> GetAllAsync();
        Task<AdvantageVM> GetByIdAsync(int id);
        Task<Advantage> GetEntityByIdAsync(int id);
        Task CreateAsync(AdvantageCreateVM request);
        Task DeleteAsync(int id);
        Task EditAsync(AdvantageEditVM request);
    }
}
