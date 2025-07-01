using AutoMapper;
using Final_MozArt.Data;
using Final_MozArt.Helpers.Extensions;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Category;
using Microsoft.EntityFrameworkCore;

namespace Final_MozArt.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public CategoryService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        public async Task<List<CategoryVM>> GetAllAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return _mapper.Map<List<CategoryVM>>(categories);
        }

        public async Task<CategoryVM> GetByIdAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(s => s.Id == id);
            return _mapper.Map<CategoryVM>(category);
        }

        public async Task CreateAsync(CategoryCreateVM request)
        {
            bool exists = await _context.Categories
                             .AnyAsync(c => c.Name.ToLower().Trim() == request.Name.ToLower().Trim());

            if (exists)
                throw new InvalidOperationException("This category is already exist..");

            string fileName = $"{Guid.NewGuid()}-{request.Photo.FileName}";
            string path = _env.GetFilePath("assets/img/home", fileName);

            var category = _mapper.Map<Category>(request);
            category.Image = fileName;

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            await request.Photo.SaveFileAsync(path);
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return;

            string path = _env.GetFilePath("assets/img/home", category.Image);
            if (File.Exists(path)) File.Delete(path);

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(CategoryEditVM request)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(s => s.Id == request.Id);
            if (category == null) throw new Exception("Category not found");

            bool nameExists = await _context.Categories
       .AnyAsync(c => c.Id != request.Id &&
                      c.Name.ToLower().Trim() == request.Name.ToLower().Trim());

            if (nameExists)
                throw new InvalidOperationException("This category is already exist.");

            string fileName = category.Image;

            if (request.Photo != null)
            {
                fileName = $"{Guid.NewGuid()}-{request.Photo.FileName}";
                string newPath = _env.GetFilePath("assets/img/home", fileName);
                string oldPath = _env.GetFilePath("assets/img/home", category.Image);

                if (File.Exists(oldPath)) File.Delete(oldPath);
                await request.Photo.SaveFileAsync(newPath);
            }

            category.Name = request.Name;
            category.Image = fileName;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Dictionary<string, int>> GetProductCountByCategoryNameAsync()
        {
            return await _context.Products
                .Where(p => p.Category != null)
                .GroupBy(p => p.Category.Name.Trim())
                .ToDictionaryAsync(g => g.Key, g => g.Count(), StringComparer.OrdinalIgnoreCase);
        }
    }
}

