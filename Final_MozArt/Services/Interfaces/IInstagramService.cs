using Final_MozArt.ViewModels.Instagram;

namespace Final_MozArt.Services.Interfaces
{
    public interface IInstagramService
    {
        Task<List<InstagramVM>> GetAllAsync();
        Task<InstagramVM> GetByIdAsync(int id);
        Task CreateAsync(InstagramCreateVM request);
        Task DeleteAsync(int id);
        Task EditAsync(InstagramEditVM request);

    }
}
