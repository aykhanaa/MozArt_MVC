using Final_MozArt.ViewModels.ContactMessage;

namespace Final_MozArt.Services.Interfaces
{
    public interface IContactMessageService
    {
        Task<bool> CreateAsync(ContactMessageCreateVM contact);

        Task<List<ContactMessageVM>> GetAllApprovedMessagesAsync();

        Task<List<ContactMessageVM>> GetAllMessagesAsync();

        Task DeleteAsync(int id);

        Task<ContactMessageVM> GetApprovedMessageByIdAsync(int id);

        Task<ContactMessageVM> GetMessageByIdAsync(int id);

        Task ApproveMessageAsync(int id);
    }
}

