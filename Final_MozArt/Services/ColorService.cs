using AutoMapper;
using Final_MozArt.Data;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Color;
using Microsoft.EntityFrameworkCore;

namespace Final_MozArt.Services
{
    public class ColorService : IColorService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public ColorService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        public async Task<List<ColorVM>> GetAllAsync()
        {
            var colors = await _context.Colors.ToListAsync();
            return _mapper.Map<List<ColorVM>>(colors);
        }

        public async Task<ColorVM> GetByIdAsync(int id)
        {
            var color = await _context.Colors.FirstOrDefaultAsync(s => s.Id == id);
            return _mapper.Map<ColorVM>(color);
        }

        public async Task<Color> GetEntityByIdAsync(int id)
        {
            return await _context.Colors.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task CreateAsync(ColorCreateVM request)
        {

            bool exists = await _context.Colors
                            .AnyAsync(c => c.Name.ToLower().Trim() == request.Name.ToLower().Trim());

            if (exists)
                throw new InvalidOperationException("This color is already exist..");

            var color = _mapper.Map<Color>(request);


            await _context.Colors.AddAsync(color);
            await _context.SaveChangesAsync();


        }

        public async Task DeleteAsync(int id)
        {
            var color = await _context.Colors.FindAsync(id);
            if (color == null) return;


            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(ColorEditVM request)
        {
            var color = await _context.Colors.FirstOrDefaultAsync(s => s.Id == request.Id);
            if (color == null) throw new Exception("Color not found");

            bool nameExists = await _context.Colors
                                            .AnyAsync(c => c.Id != request.Id &&
                    c.Name.ToLower().Trim() == request.Name.ToLower().Trim());

            if (nameExists)
                throw new InvalidOperationException("This color is already exist.");

            color.Name = request.Name;

            _context.Colors.Update(color);
            await _context.SaveChangesAsync();
        }
    }
}
