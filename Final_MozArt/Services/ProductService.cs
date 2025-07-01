using AutoMapper;
using Final_MozArt.Data;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Blog;
using Final_MozArt.ViewModels.Category;
using Final_MozArt.ViewModels.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Final_MozArt.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly string _imageFolderPath = "wwwroot/assets/img/product-details";
        private readonly IEmailService _emailService;

        public ProductService(AppDbContext context, IMapper mapper, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;

            if (!Directory.Exists(_imageFolderPath))
                Directory.CreateDirectory(_imageFolderPath);
            _emailService = emailService;
        }
        public async Task<List<ProductVM>> GetProductsByIdsAsync(List<int> ids)
        {
            if (ids == null || !ids.Any())
                return new List<ProductVM>();

            return await _context.Products
                .Where(p => ids.Contains(p.Id))
                .Select(p => new ProductVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Image=p.Images.FirstOrDefault(m=>m.IsMain).Image
                })
                .ToListAsync();
        }

        public async Task<ICollection<ProductVM>> GetAllAsync()
        {
            var products = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<ICollection<ProductVM>>(products);
        }

        public async Task<ProductVM> GetByIdWithIncludesAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return null;

            return _mapper.Map<ProductVM>(product);
        }

        public async Task<ProductDetailVM> GetByIdWithIncludesWithoutTrackingAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductColors).ThenInclude(pc => pc.Color)
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return null;

            var detailVM = new ProductDetailVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CreateDate = product.CreateDate,

                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,

                BrandId = product.BrandId,
                BrandName = product.Brand?.Name,

                ColorIds = product.ProductColors.Select(pc => pc.ColorId).ToList(),
                ColorNames = product.ProductColors.Select(pc => pc.Color.Name).ToList(),

                TagIds = product.ProductTags.Select(pt => pt.TagId).ToList(),
                TagNames = product.ProductTags.Select(pt => pt.Tag.Name).ToList(),

                Images = product.Images?.ToList()
            };

            return detailVM;
        }


        public async Task<ICollection<ProductVM>> GetPaginatedDatasByCategory(int categoryId, int page, int take)
        {
            var products = await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .AsNoTracking()
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();

            return _mapper.Map<ICollection<ProductVM>>(products);
        }

        public async Task<ICollection<ProductVM>> GetPaginatedDatasAsync(int page, int take)
        {
            var products = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .AsNoTracking()
                .OrderByDescending(p => p.CreateDate)
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();

            return _mapper.Map<ICollection<ProductVM>>(products);
        }

        public async Task<ProductVM> GetByNameWithoutTrackingAsync(string name)
        {
            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());

            return product == null ? null : _mapper.Map<ProductVM>(product);
        }

        public async Task<int> GetCountAsync()
            => await _context.Products.CountAsync();

        public async Task<int> GetCountByCategoryAsync(int categoryId)
            => await _context.Products.Where(p => p.CategoryId == categoryId).CountAsync();

        public async Task<int> GetCountBySearch(string searchText)
            => await _context.Products
                .Where(p => p.Name.Contains(searchText) || p.Description.Contains(searchText))
                .CountAsync();

        public async Task<ICollection<ProductVM>> SearchAsync(string searchText, int page, int take)
        {
            var products = await _context.Products
                .Where(p => p.Name.Contains(searchText) || p.Description.Contains(searchText))
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .AsNoTracking()
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();

            return _mapper.Map<ICollection<ProductVM>>(products);
        }


        public async Task CreateAsync(ProductCreateVM vm)
        {
            // VM-dən Product obyektinə xəritələmə
            var product = _mapper.Map<Product>(vm);
            product.Images = new List<ProductImage>();

            // Rəng əlaqələri
            product.ProductColors = vm.ColorIds.Select(colorId => new ProductColor
            {
                ColorId = colorId,
                Product = product
            }).ToList();

            // Tag əlaqələri
            product.ProductTags = vm.TagIds.Select(tagId => new ProductTag
            {
                TagId = tagId,
                Product = product
            }).ToList();

            // Şəkillərin əlavə olunması
            var photos = vm.Photos.ToList();
            for (int i = 0; i < photos.Count; i++)
            {
                var photo = photos[i];
                var fileName = await SaveFileAsync(photo);
                product.Images.Add(new ProductImage
                {
                    Image = fileName,
                    IsMain = i == 0,
                    IsHover = i == 1,
                    Product = product
                });
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var subscribers = await _context.Subscribes.Select(s => s.Email).ToListAsync();

            if (subscribers.Count > 0)
            {
                string subject = "Our New Product is Now Available!";
                string productName = product.Name;
                string productLink = $"https://localhost:7286/Shop/Detail/{product.Id}";

                string body = $@"
  <div style='font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 30px; border-radius: 10px; max-width: 600px; margin: auto; box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);'>
    <h1 style='color: #2c3e50;'>🎉 Meet Our New Product!</h1>
    <p style='font-size: 16px; color: #333;'>Salam!</p>
    <p style='font-size: 16px; color: #333;'>
      We are excited to introduce you to our latest product, <strong>{productName}</strong>!
    </p>
    <p style='font-size: 16px; color: #333;'>
      To view all product details and place an order, click on the link below:
    </p>
    <div style='text-align: center; margin: 20px 0;'>
      <a href='{productLink}' style='background-color: #3498db; color: white; padding: 12px 24px; border-radius: 6px; text-decoration: none; font-weight: bold;'>View Product</a>
    </div>
    <p style='font-size: 16px; color: #333;'>We are always at your service!</p>
    <p style='font-size: 16px; color: #333;'><em>MozArt team</em></p>
  </div>";


                foreach (var email in subscribers)
                {
                    _emailService.Send(email, subject, body);
                }
            }
        }


        public async Task EditAsync(ProductEditVM vm)
        {
            var product = await _context.Products
                .Include(p => p.ProductColors)
                .Include(p => p.ProductTags)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == vm.Id);

            if (product == null)
                throw new KeyNotFoundException("Product tapilmadi.");

            product.Name = vm.Name;
            product.Price = vm.Price;
            product.Description = vm.Description;
            product.BrandId = vm.BrandId;
            product.CategoryId = vm.CategoryId;

            _context.ProductColors.RemoveRange(product.ProductColors);
            product.ProductColors = vm.ColorIds.Select(colorId => new ProductColor
            {
                ColorId = colorId,
                ProductId = product.Id
            }).ToList();

            _context.ProductTags.RemoveRange(product.ProductTags);
            product.ProductTags = vm.TagIds.Select(tagId => new ProductTag
            {
                TagId = tagId,
                ProductId = product.Id
            }).ToList();

            if (vm.Photos != null && vm.Photos.Any())
            {
                product.Images.Clear();
                var photos = vm.Photos.ToList();
                for (int i = 0; i < photos.Count; i++)
                {
                    var photo = photos[i];
                    var fileName = await SaveFileAsync(photo);
                    product.Images.Add(new ProductImage
                    {
                        Image = fileName,
                        IsMain = i == 0,
                        IsHover = i == 1,
                        Product = product
                    });
                }
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                throw new KeyNotFoundException("Product tapilmadi.");

            foreach (var image in product.Images)
            {
                DeleteFileIfExists(image.Image);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductImageAsync(int id)
        {
            var image = await _context.ProductImages.FindAsync(id);
            if (image == null)
                throw new KeyNotFoundException("Image tapilmadi.");

            DeleteFileIfExists(image.Image);

            _context.ProductImages.Remove(image);
            await _context.SaveChangesAsync();
        }


        public async Task SetMainImageAsync(SetIsMainVM data)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == data.ProductId);

            if (product == null)
                throw new KeyNotFoundException("Product tapilmadi.");

            var selectedImage = product.Images.FirstOrDefault(x => x.Id == data.ImageId);

            if (selectedImage == null)
                throw new KeyNotFoundException("Image tapilmadi.");

            if (selectedImage.IsHover)
            {
                return;
            }

            foreach (var img in product.Images)
            {
                img.IsMain = img.Id == data.ImageId;
            }

            await _context.SaveChangesAsync();
        }


        private async Task<string> SaveFileAsync(IFormFile file)
        {
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(_imageFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        private void DeleteFileIfExists(string fileName)
        {
            var filePath = Path.Combine(_imageFolderPath, fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
        public async Task<List<Product>> SearchProductsAsync(string query)
        {
            IQueryable<Product> products = _context.Products.Include(p => p.Category);

            if (!string.IsNullOrEmpty(query))
            {
                query = query.ToLower();

                products = products.Where(p =>
                    p.Name.ToLower().Contains(query) ||
                    p.Category.Name.ToLower().Contains(query));
            }

            return await products.ToListAsync();
        }

        public async Task<ICollection<ProductVM>> SortAsync(string sortKey)
        {
            IQueryable<Product> query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .AsNoTracking();

            switch (sortKey)
            {
                case "A-Z":
                    query = query.OrderBy(p => p.Name);
                    break;
                case "Z-A":
                    query = query.OrderByDescending(p => p.Name);
                    break;
                case "LowToHigh":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "HighToLow":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                default:
                    query = query.OrderByDescending(p => p.CreateDate);
                    break;
            }

            var products = await query.ToListAsync();

            return _mapper.Map<ICollection<ProductVM>>(products);
        }

        //public async Task<ICollection<ProductVM>> FilterAsync(string? categoryName, string? brandName, string? tagName)
        //{
        //    IQueryable<Product> query = _context.Products
        //        .Include(p => p.Category)
        //        .Include(p => p.Brand)
        //        .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
        //        .Include(p => p.Images)
        //        .AsNoTracking();

        //    if (!string.IsNullOrWhiteSpace(categoryName))
        //    {
        //        categoryName = categoryName.Trim();
        //        query = query.Where(p => p.Category != null && p.Category.Name == categoryName);
        //    }

        //    if (!string.IsNullOrWhiteSpace(brandName))
        //    {
        //        brandName = brandName.Trim();
        //        query = query.Where(p => p.Brand != null && p.Brand.Name == brandName);
        //    }

        //    if (!string.IsNullOrWhiteSpace(tagName))
        //    {
        //        tagName = tagName.Trim();
        //        query = query.Where(p => p.ProductTags.Any(pt => pt.Tag != null && pt.Tag.Name == tagName));
        //    }

        //    var filteredProducts = await query.ToListAsync();
        //    return _mapper.Map<ICollection<ProductVM>>(filteredProducts);
        //}


        public async Task<ICollection<ProductVM>> FilterAsync(string? categoryName, string? brandName, string? tagName)
        {
            IQueryable<Product> query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.Images)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                categoryName = categoryName.Trim();
                // Case-insensitive comparison əlavə edin
                query = query.Where(p => p.Category != null &&
                           EF.Functions.Like(p.Category.Name, categoryName));

            }

            if (!string.IsNullOrWhiteSpace(brandName))
            {
                brandName = brandName.Trim();
                // Case-insensitive comparison əlavə edin
                query = query.Where(p => p.Brand != null &&
                                   p.Brand.Name.ToLower() == brandName.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(tagName))
            {
                tagName = tagName.Trim();
                // Case-insensitive comparison əlavə edin
                query = query.Where(p => p.ProductTags.Any(pt => pt.Tag != null &&
                                   pt.Tag.Name.ToLower() == tagName.ToLower()));
            }

            var filteredProducts = await query.ToListAsync();
            return _mapper.Map<ICollection<ProductVM>>(filteredProducts);
        }

        public async Task<List<ProductVM>> GetProductsAsync(int skip, int take)
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Brand)
                    .OrderByDescending(p => p.CreateDate)
                    .Skip(skip)
                    .Take(take)
                    .Select(p => new ProductVM
                    {
                        Id = p.Id,
                        Name = p.Name ?? "",
                        Description = p.Description ?? "",
                        Price = p.Price,
                        CategoryName = p.Category != null ? p.Category.Name : "",
                        BrandName = p.Brand != null ? p.Brand.Name : "",
                        CreateDate = p.CreateDate
                    })
                    .ToListAsync();

                return products;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetProductsAsync Error: {ex.Message}");
                return new List<ProductVM>();
            }
        }


        public async Task<int> GetTotalProductCountAsync()
        {
            try
            {
                return await _context.Products.CountAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetTotalProductCountAsync Error: {ex.Message}");
                return 0;
            }
        }

        public Task<List<ProductVM>> GetProductAsync(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductVM>> FilterByPriceAsync(decimal min, decimal max)
        {
           
            var products = await _context.Products
               .Where(p => p.Price >= min && p.Price <= max)
               .Include(p => p.Brand)
               .Include(p => p.Category)
               .Include(p => p.Images)
               .AsNoTracking()
              
               .ToListAsync();

            return _mapper.Map<List<ProductVM>>(products);
        }
    }
}
