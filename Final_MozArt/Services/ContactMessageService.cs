using AutoMapper;
using Final_MozArt.Data;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.ContactMessage;
using Microsoft.EntityFrameworkCore;

namespace Final_MozArt.Services
{
    public class ContactMessageService : IContactMessageService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ContactMessageService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(ContactMessageCreateVM contact)
        {
            var isRegistered = await _context.Users.AnyAsync(u => u.Email == contact.Email);

            if (!isRegistered) return false;

            var entity = _mapper.Map<ContactMessage>(contact);
            entity.IsApproved = false;
            entity.CreateDate = DateTime.UtcNow;

            await _context.ContactMessages.AddAsync(entity);
            await _context.SaveChangesAsync();

            return true;
        }



        public async Task<List<ContactMessageVM>> GetAllApprovedMessagesAsync()
        {
            var messages = await _context.ContactMessages
                .Where(m => m.IsApproved)
                .ToListAsync();

            return _mapper.Map<List<ContactMessageVM>>(messages);
        }

        public async Task<List<ContactMessageVM>> GetAllMessagesAsync()
        {
            var messages = await _context.ContactMessages.ToListAsync();
            return _mapper.Map<List<ContactMessageVM>>(messages);
        }

        public async Task DeleteAsync(int id)
        {

            ContactMessage dbContactMessage = await _context.ContactMessages.FirstOrDefaultAsync(m => m.Id == id);
            _context.ContactMessages.Remove(dbContactMessage);
            await _context.SaveChangesAsync();
        }

        public async Task<ContactMessageVM> GetApprovedMessageByIdAsync(int id)
        {
            var entity = await _context.ContactMessages
                .FirstOrDefaultAsync(m => m.Id == id && m.IsApproved);

            return _mapper.Map<ContactMessageVM>(entity);
        }

        public async Task<ContactMessageVM> GetMessageByIdAsync(int id)
        {
            var entity = await _context.ContactMessages.FindAsync(id);
            return _mapper.Map<ContactMessageVM>(entity);
        }

        public async Task ApproveMessageAsync(int id)
        {
            var entity = await _context.ContactMessages.FindAsync(id);
            if (entity != null)
            {
                entity.IsApproved = true; 
                await _context.SaveChangesAsync();
            }
        }
    }
}
