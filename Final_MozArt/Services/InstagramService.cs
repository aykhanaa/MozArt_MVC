using AutoMapper;
using Final_MozArt.Data;
using Final_MozArt.Helpers.Extensions;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Instagram;
using Microsoft.EntityFrameworkCore;

namespace Final_MozArt.Services
{
    public class InstagramService : IInstagramService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public InstagramService (AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        public async Task<List<InstagramVM>> GetAllAsync()
        {
            var instagrams = await _context.Instagrams.ToListAsync();
            return _mapper.Map<List<InstagramVM>>(instagrams);
        }

        public async Task<InstagramVM> GetByIdAsync(int id)
        {
            var instagram = await _context.Instagrams.FirstOrDefaultAsync(s => s.Id == id);
            return _mapper.Map<InstagramVM>(instagram);
        }

        public async Task CreateAsync(InstagramCreateVM request)
        {
            string fileName = $"{Guid.NewGuid()}-{request.Photo.FileName}";
            string path = _env.GetFilePath("assets/img/home", fileName);

            var instagram = _mapper.Map<Instagram>(request);
            instagram.Image = fileName;

            await _context.Instagrams.AddAsync(instagram);
            await _context.SaveChangesAsync();

            await request.Photo.SaveFileAsync(path);
        }

        public async Task DeleteAsync(int id)
        {
            var instagram = await _context.Instagrams.FindAsync(id);
            if (instagram == null) return;

            string path = _env.GetFilePath("assets/img/home", instagram.Image);
            if (File.Exists(path)) File.Delete(path);

            _context.Instagrams.Remove(instagram);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(InstagramEditVM request)
        {
            var instagram = await _context.Instagrams.FirstOrDefaultAsync(s => s.Id == request.Id);
            if (instagram == null) throw new Exception("Instagram not found");

            string fileName = instagram.Image;

            if (request.Photo != null)
            {
                fileName = $"{Guid.NewGuid()}-{request.Photo.FileName}";
                string newPath = _env.GetFilePath("assets/img/home", fileName);
                string oldPath = _env.GetFilePath("assets/img/home", instagram.Image);

                if (File.Exists(oldPath)) File.Delete(oldPath);
                await request.Photo.SaveFileAsync(newPath);
            }

            instagram.Image = fileName;

            _context.Instagrams.Update(instagram);
            await _context.SaveChangesAsync();
        }
    }
}
