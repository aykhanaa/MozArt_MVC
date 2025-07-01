using AutoMapper;
using Final_MozArt.Data;
using Final_MozArt.Helpers.Extensions;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.About;
using Microsoft.EntityFrameworkCore;

namespace Final_MozArt.Services
{
    public class AboutService : IAboutService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public AboutService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        public async Task<List<AboutVM>> GetAllAsync()
        {
            var abouts = await _context.Abouts.ToListAsync();
            return _mapper.Map<List<AboutVM>>(abouts);
        }

        public async Task<AboutVM> GetByIdAsync(int id)
        {
            var about = await _context.Abouts.FirstOrDefaultAsync(s => s.Id == id);
            return _mapper.Map<AboutVM>(about);
        }

        public async Task<About> GetEntityByIdAsync(int id)
        {
            return await _context.Abouts.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task CreateAsync(AboutCreateVM request)
        {
            string fileName = $"{Guid.NewGuid()}-{request.Photo.FileName}";
            string path = _env.GetFilePath("assets/img/about", fileName);

            var about = _mapper.Map<About>(request);
            about.Image = fileName;

            await _context.Abouts.AddAsync(about);
            await _context.SaveChangesAsync();

            await request.Photo.SaveFileAsync(path);
        }

        public async Task DeleteAsync(int id)
        {
            var about = await _context.Abouts.FindAsync(id);
            if (about == null) return;

            string path = _env.GetFilePath("assets/img/about", about.Image);
            if (File.Exists(path)) File.Delete(path);

            _context.Abouts.Remove(about);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(AboutEditVM request)
        {
            var about = await _context.Abouts.FirstOrDefaultAsync(s => s.Id == request.Id);
            if (about == null) throw new Exception("Support not found");

            string fileName = about.Image;

            if (request.Photo != null)
            {
                fileName = $"{Guid.NewGuid()}-{request.Photo.FileName}";
                string newPath = _env.GetFilePath("assets/img/home", fileName);
                string oldPath = _env.GetFilePath("assets/img/home", about.Image);

                if (File.Exists(oldPath)) File.Delete(oldPath);
                await request.Photo.SaveFileAsync(newPath);
            }

            about.Title = request.Title;
            about.Subtitle = request.Subtitle;
            about.Description = request.Description;
            about.Image = fileName;

            _context.Abouts.Update(about        );
            await _context.SaveChangesAsync();
        }
    }
}
