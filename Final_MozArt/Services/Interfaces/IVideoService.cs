using Final_MozArt.ViewModels.Video;

namespace Final_MozArt.Services.Interfaces
{
    public interface IVideoService
    {
        Task<List<VideoVM>> GetAllAsync();
        Task<VideoVM> GetByIdAsync(int id);
        Task CreateAsync(VideoCreateVM request);
        Task DeleteAsync(int id);
        Task EditAsync(VideoEditVM request);
    }
}
