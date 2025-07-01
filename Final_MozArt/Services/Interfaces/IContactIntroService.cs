using Final_MozArt.Models;
using Final_MozArt.ViewModels.ContactIntro;
using Final_MozArt.ViewModels.Support;

namespace Final_MozArt.Services.Interfaces
{
    public interface IContactIntroService
    {
        Task<List<ContactIntroVM>> GetAllAsync();
        Task<ContactIntroVM> GetByIdAsync(int id);
        Task<ContactIntro> GetEntityByIdAsync(int id);
        Task CreateAsync(ContactIntroCreateVM request);
        Task DeleteAsync(int id);
        Task EditAsync(ContactIntroEditVM request);
    }
}
