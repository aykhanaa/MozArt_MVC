using AutoMapper;
using Final_MozArt.Data;
using Final_MozArt.Helpers.Extensions;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Slider;
using Final_MozArt.ViewModels.Support;
using Microsoft.EntityFrameworkCore;

namespace Final_MozArt.Services
{
    public class SupportService : ISupportService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public SupportService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        public async Task<List<SupportVM>> GetAllAsync()
        {
            var supports = await _context.Supports.ToListAsync();
            return _mapper.Map<List<SupportVM>>(supports);
        }

        public async Task<SupportVM> GetByIdAsync(int id)
        {
            var support = await _context.Supports.FirstOrDefaultAsync(s => s.Id == id);
            return _mapper.Map<SupportVM>(support);
        }

        public async Task<Support> GetEntityByIdAsync(int id)
        {
            return await _context.Supports.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task CreateAsync(SupportCreateVM request)
        {
            var support = _mapper.Map<Support>(request);
           

            await _context.Supports.AddAsync(support);
            await _context.SaveChangesAsync();

            
        }

        public async Task DeleteAsync(int id)
        {
            var support = await _context.Supports.FindAsync(id);
            if (support == null) return;

          
            _context.Supports.Remove(support);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(SupportEditVM request)
        {
            var support = await _context.Supports.FirstOrDefaultAsync(s => s.Id == request.Id);
            if (support == null) throw new Exception("Support not found");

           

            support.Title = request.Title;
            support.Description = request.Description;
        
            _context.Supports.Update(support);
            await _context.SaveChangesAsync();
        }
    }
}
