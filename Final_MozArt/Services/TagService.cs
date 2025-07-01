using AutoMapper;
using Final_MozArt.Data;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Tag;
using Microsoft.EntityFrameworkCore;

namespace Final_MozArt.Services
{
    public class TagService : ITagService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public TagService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        public async Task<List<TagVM>> GetAllAsync()
        {
            var tags = await _context.Tags.ToListAsync();
            return _mapper.Map<List<TagVM>>(tags);
        }

        public async Task<TagVM> GetByIdAsync(int id)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(s => s.Id == id);
            return _mapper.Map<TagVM>(tag);
        }

        public async Task<Tag> GetEntityByIdAsync(int id)
        {
            return await _context.Tags.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task CreateAsync(TagCreateVM request)
        {
            bool exists = await _context.Tags
                            .AnyAsync(c => c.Name.ToLower().Trim() == request.Name.ToLower().Trim());

            if (exists)
                throw new InvalidOperationException("This tags is already exist..");
            var tag = _mapper.Map<Tag>(request);

            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();


        }

        public async Task DeleteAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return;


            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(TagEditVM request)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(s => s.Id == request.Id);
            if (tag == null) throw new Exception("Tag not found");
            bool nameExists = await _context.Tags
                                            .AnyAsync(c => c.Id != request.Id &&
                     c.Name.ToLower().Trim() == request.Name.ToLower().Trim());

            if (nameExists)
                throw new InvalidOperationException("This category is already exist.");


            tag.Name = request.Name;

            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();
        }
    }
}
