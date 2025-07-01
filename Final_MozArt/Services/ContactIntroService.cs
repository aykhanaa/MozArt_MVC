using AutoMapper;
using Final_MozArt.Data;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.ContactIntro;
using Microsoft.EntityFrameworkCore;

namespace Final_MozArt.Services
{
    public class ContactIntroService : IContactIntroService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public ContactIntroService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        public async Task<List<ContactIntroVM>> GetAllAsync()
        {
            var contactintros = await _context.ContactIntros.ToListAsync();
            return _mapper.Map<List<ContactIntroVM>>(contactintros);
        }

        public async Task<ContactIntroVM> GetByIdAsync(int id)
        {
            var contactintro = await _context.ContactIntros.FirstOrDefaultAsync(s => s.Id == id);
            return _mapper.Map<ContactIntroVM>(contactintro);
        }

        public async Task<ContactIntro> GetEntityByIdAsync(int id)
        {
            return await _context.ContactIntros.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task CreateAsync(ContactIntroCreateVM request)
        {
            var contactintro = _mapper.Map<ContactIntro>(request);


            await _context.ContactIntros.AddAsync(contactintro);
            await _context.SaveChangesAsync();


        }

        public async Task DeleteAsync(int id)
        {
            var contactintro = await _context.ContactIntros.FindAsync(id);
            if (contactintro == null) return;


            _context.ContactIntros.Remove(contactintro);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(ContactIntroEditVM request)
        {
            var contactintro = await _context.ContactIntros.FirstOrDefaultAsync(s => s.Id == request.Id);
            if (contactintro == null) throw new Exception("Contact intro not found");



            contactintro.Title = request.Title;
            contactintro.Description = request.Description;

            _context.ContactIntros.Update(contactintro);
            await _context.SaveChangesAsync();
        }
    }
}
