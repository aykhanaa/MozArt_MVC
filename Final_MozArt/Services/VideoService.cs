using AutoMapper;
using Final_MozArt.Data;
using Final_MozArt.Helpers.Extensions;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Video;
using Microsoft.EntityFrameworkCore;

namespace Final_MozArt.Services
{
    public class VideoService : IVideoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public VideoService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        public async Task<List<VideoVM>> GetAllAsync()
        {
            var videos = await _context.Videos.ToListAsync();
            return _mapper.Map<List<VideoVM>>(videos);
        }

        public async Task<VideoVM> GetByIdAsync(int id)
        {
            var video = await _context.Videos.FirstOrDefaultAsync(s => s.Id == id);
            return _mapper.Map<VideoVM>(video);
        }

        public async Task CreateAsync(VideoCreateVM request)
        {
            string fileName = $"{Guid.NewGuid()}-{request.Photo.FileName}";
            string path = _env.GetFilePath("assets/img/about", fileName);

            var video = _mapper.Map<Video>(request);
            video.Image = fileName;

            await _context.Videos.AddAsync(video);
            await _context.SaveChangesAsync();

            await request.Photo.SaveFileAsync(path);
        }

        public async Task DeleteAsync(int id)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video == null) return;

            string path = _env.GetFilePath("assets/img/about", video.Image);
            if (File.Exists(path)) File.Delete(path);

            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(VideoEditVM request)
        {
            var video = await _context.Videos.FirstOrDefaultAsync(s => s.Id == request.Id);
            if (video == null) throw new Exception("Video not found");

            string fileName = video.Image;

            if (request.Photo != null)
            {
                fileName = $"{Guid.NewGuid()}-{request.Photo.FileName}";
                string newPath = _env.GetFilePath("assets/img/about", fileName);
                string oldPath = _env.GetFilePath("assets/img/about", video.Image);

                if (File.Exists(oldPath)) File.Delete(oldPath);
                await request.Photo.SaveFileAsync(newPath);
            }

            video.VideoURL = request.VideoURL;
            video.Image = fileName;

            _context.Videos.Update(video);
            await _context.SaveChangesAsync();
        }
    }
}
