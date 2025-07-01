using AutoMapper;
using Final_MozArt.Data;
using Final_MozArt.Helpers.Extensions;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Slider;
using Microsoft.EntityFrameworkCore;


namespace Final_MozArt.Services
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public SliderService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        public async Task<List<SliderVM>> GetAllAsync()
        {
            var sliders = await _context.Sliders.ToListAsync();
            return _mapper.Map<List<SliderVM>>(sliders);
        }

        public async Task<SliderVM> GetByIdAsync(int id)
        {
            var slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            return _mapper.Map<SliderVM>(slider);
        }

        public async Task<Slider> GetEntityByIdAsync(int id)
        {
            return await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task CreateAsync(SliderCreateVM request)
        {
            string fileName = $"{Guid.NewGuid()}-{request.Photo.FileName}";
            string path = _env.GetFilePath("assets/img/home", fileName);

            var slider = _mapper.Map<Slider>(request);
            slider.Image = fileName;

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            await request.Photo.SaveFileAsync(path);
        }

        public async Task DeleteAsync(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return;

            string path = _env.GetFilePath("assets/img/home", slider.Image);
            if (File.Exists(path)) File.Delete(path);

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(SliderEditVM request)
        {
            var slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == request.Id);
            if (slider == null) throw new Exception("Slider not found");

            string fileName = slider.Image;

            if (request.Photo != null)
            {
                fileName = $"{Guid.NewGuid()}-{request.Photo.FileName}";
                string newPath = _env.GetFilePath("assets/img/home", fileName);
                string oldPath = _env.GetFilePath("assets/img/home", slider.Image);

                if (File.Exists(oldPath)) File.Delete(oldPath);
                await request.Photo.SaveFileAsync(newPath);
            }

            slider.Title = request.Title;       
            slider.Image = fileName;           

            _context.Sliders.Update(slider);
            await _context.SaveChangesAsync();
        }


    }
}
