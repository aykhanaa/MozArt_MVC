using AutoMapper;
using Final_MozArt.Data;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.BlogCategory;
using Microsoft.EntityFrameworkCore;

namespace Final_MozArt.Services
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BlogCategoryService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<BlogCategoryVM>> GetAllAsync()
        {
            var categories = await _context.BlogCategories.ToListAsync();
            return _mapper.Map<List<BlogCategoryVM>>(categories);
        }

        public async Task<BlogCategoryVM> GetByIdAsync(int id)
        {
            var category = await _context.BlogCategories.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<BlogCategoryVM>(category);
        }

        public async Task<BlogCategory> GetEntityByIdAsync(int id)
        {
            return await _context.BlogCategories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(BlogCategoryCreateVM request)
        {
            var category = _mapper.Map<BlogCategory>(request);
            await _context.BlogCategories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.BlogCategories.FindAsync(id);
            if (category == null) return;

            _context.BlogCategories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(BlogCategoryEditVM request)
        {
            var category = await _context.BlogCategories.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (category == null) throw new Exception("Blog category not found");

            category.Name = request.Name;

            _context.BlogCategories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
