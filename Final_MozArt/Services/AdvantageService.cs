using AutoMapper;
using Final_MozArt.Data;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Advantage;
using Microsoft.EntityFrameworkCore;

namespace Final_MozArt.Services
{
    public class AdvantageService : IAdvantageService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public AdvantageService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }


        public async Task<List<AdvantageVM>> GetAllAsync()
        {
            var advantages = await _context.Advantages.ToListAsync();
            return _mapper.Map<List<AdvantageVM>>(advantages);
        }

        public async Task<AdvantageVM> GetByIdAsync(int id)
        {
            var advantage = await _context.Advantages.FirstOrDefaultAsync(s => s.Id == id);
            return _mapper.Map<AdvantageVM>(advantage);
        }

        public async Task<Advantage> GetEntityByIdAsync(int id)
        {
            return await _context.Advantages.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task CreateAsync(AdvantageCreateVM request)
        {
            var advantage = _mapper.Map<Advantage>(request);


            await _context.Advantages.AddAsync(advantage);
            await _context.SaveChangesAsync();


        }

        public async Task DeleteAsync(int id)
        {
            var advantage = await _context.Advantages.FindAsync(id);
            if (advantage == null) return;


            _context.Advantages.Remove(advantage);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(AdvantageEditVM request)
        {
            var advantage = await _context.Advantages.FirstOrDefaultAsync(s => s.Id == request.Id);
            if (advantage == null) throw new Exception("Advantage not found");



            advantage.Title = request.Title;
            advantage.Description = request.Description;

            _context.Advantages.Update(advantage);
            await _context.SaveChangesAsync();
        }
    }
}
