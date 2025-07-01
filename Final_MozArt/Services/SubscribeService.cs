using Final_MozArt.Data;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Final_MozArt.Services
{
    public class SubscribeService : ISubscribeService
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public SubscribeService(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task AddAsync(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                var newsletter = new Subscribe { Email = email };
                _context.Subscribes.Add(newsletter);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<List<Subscribe>> GetAllAsync()
        {
            return await _context.Subscribes
                .OrderByDescending(n => n.CreateDate)
                .ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {

            var subscribe = await _context.Subscribes.FirstOrDefaultAsync(m => m.Id == id);
            _context.Subscribes.Remove(subscribe);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _context.Subscribes.AnyAsync(n => n.Email.ToLower() == email.ToLower());
        }

    }
}
    



