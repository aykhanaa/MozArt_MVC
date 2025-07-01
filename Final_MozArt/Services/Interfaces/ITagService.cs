using Final_MozArt.Models;
using Final_MozArt.ViewModels.Support;
using Final_MozArt.ViewModels.Tag;

namespace Final_MozArt.Services.Interfaces
{
    public interface ITagService
    {
        Task<List<TagVM>> GetAllAsync();
        Task<TagVM> GetByIdAsync(int id);
        Task<Tag> GetEntityByIdAsync(int id);
        Task CreateAsync(TagCreateVM request);
        Task DeleteAsync(int id);
        Task EditAsync(TagEditVM request);
    }
}
