using AutoMapper;
using Final_MozArt.Data;
using Final_MozArt.Helpers.Extensions;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Brand;
using Microsoft.EntityFrameworkCore;

namespace Final_MozArt.Services
{
    public class BrandService : IBrandService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public BrandService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        public async Task<List<BrandVM>> GetAllAsync()
        {
            var brands = await _context.Brands.ToListAsync();
            return _mapper.Map<List<BrandVM>>(brands);
        }

        public async Task<BrandVM> GetByIdAsync(int id)
        {
            var brand = await _context.Brands.FirstOrDefaultAsync(s => s.Id == id);
            return _mapper.Map<BrandVM>(brand);
        }

        public async Task CreateAsync(BrandCreateVM request)
        {

            bool exists = await _context.Brands.AnyAsync(c => c.Name.ToLower().Trim() == request.Name.ToLower().Trim());

            if (exists)
                throw new InvalidOperationException("This brands is already exist");
            string fileName = $"{Guid.NewGuid()}-{request.Photo.FileName}";
            string path = _env.GetFilePath("assets/img/home", fileName);

            var brand = _mapper.Map<Brand>(request);
            brand.Image = fileName;

            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();

            await request.Photo.SaveFileAsync(path);
        }

        public async Task DeleteAsync(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null) return;

            string path = _env.GetFilePath("assets/img/home", brand.Image);
            if (File.Exists(path)) File.Delete(path);

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(BrandEditVM request)
        {
            var brand = await _context.Brands.FirstOrDefaultAsync(s => s.Id == request.Id);
            if (brand == null) throw new Exception("Brand not found");

            bool nameExists = await _context.Brands
      .AnyAsync(c => c.Id != request.Id &&
                     c.Name.ToLower().Trim() == request.Name.ToLower().Trim());

            if (nameExists)
                throw new InvalidOperationException("This brands is already exist.");

            string fileName = brand.Image;

            if (request.Photo != null)
            {
                fileName = $"{Guid.NewGuid()}-{request.Photo.FileName}";
                string newPath = _env.GetFilePath("assets/img/home", fileName);
                string oldPath = _env.GetFilePath("assets/img/home", brand.Image);

                if (File.Exists(oldPath)) File.Delete(oldPath);
                await request.Photo.SaveFileAsync(newPath);
            }

            brand.Name = request.Name;
            brand.Image = fileName;

            _context.Brands.Update(brand);
            await _context.SaveChangesAsync();
        }

        //public async Task<Dictionary<string, int>> GetProductCountByBrandNameAsync()
        //{
        //    var result = await _context.Brands
        //        .Select(c => new
        //        {
        //            BrandName = c.Name,
        //            ProductCount = _context.Products.Count(p => p.CategoryId == c.Id)
        //        })
        //        .ToDictionaryAsync(x => x.BrandName, x => x.ProductCount);

        //    return result;
        //}

        public async Task<Dictionary<string, int>> GetProductCountByBrandNameAsync()
        {
            return await _context.Products
                .Where(p => p.Brand != null)
                .GroupBy(p => p.Brand.Name.Trim())
                .ToDictionaryAsync(g => g.Key, g => g.Count(), StringComparer.OrdinalIgnoreCase);
        }
    }
}
