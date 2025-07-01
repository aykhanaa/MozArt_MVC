using AutoMapper;
using Final_MozArt.Data;
using Final_MozArt.Helpers.Extensions;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Blog;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Final_MozArt.Services
{
    public class BlogService : IBlogService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly ISettingService _settingService;

        public BlogService(AppDbContext context, IMapper mapper, IWebHostEnvironment env, ISettingService settingService)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
            _settingService = settingService;
        }

        public async Task<List<BlogVM>> GetAllAsync()
        {
            var blogs = await _context.Blogs.Include(b => b.BlogCategory).ToListAsync();
            return _mapper.Map<List<BlogVM>>(blogs);
        }

        public async Task<BlogVM> GetByIdAsync(int id)
        {
            var blog = await _context.Blogs.Include(b => b.BlogCategory).FirstOrDefaultAsync(b => b.Id == id);
            var vm= _mapper.Map<BlogVM>(blog);
            //vm.Banner =   _settingService.GetSettingValue("BlogBanner");
            return vm;
        }

        public async Task<Blog> GetEntityByIdAsync(int id)
        {
            return await _context.Blogs.Include(b => b.BlogCategory).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task CreateAsync(BlogCreateVM request)
        {
            string fileName = $"{Guid.NewGuid()}-{request.Photo.FileName}";
            string path = _env.GetFilePath("assets/img/blog", fileName);

            var blog = _mapper.Map<Blog>(request);
            blog.Image = fileName;

            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();

            await request.Photo.SaveFileAsync(path);
        }

        public async Task EditAsync(BlogEditVM request)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == request.Id);
            if (blog == null) throw new Exception("Blog not found");

            string fileName = blog.Image;

            if (request.Photo != null)
            {
                fileName = $"{Guid.NewGuid()}-{request.Photo.FileName}";
                string newPath = _env.GetFilePath("assets/img/blog", fileName);
                string oldPath = _env.GetFilePath("assets/img/blog", blog.Image);

                if (File.Exists(oldPath)) File.Delete(oldPath);
                await request.Photo.SaveFileAsync(newPath);
            }

            blog.Title = request.Title;
            blog.Description = request.Description;
            blog.Image = fileName;
            blog.BlogCategoryId = request.BlogCategoryId;

            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null) return;

            string path = _env.GetFilePath("assets/img/blog", blog.Image);
            if (File.Exists(path)) File.Delete(path);

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
        }

        public async Task<List<BlogVM>> GetBlogsAsync(int skip, int take)
        {
            try
            {
                var blogs = await _context.Blogs.Include(b=>b.BlogCategory)
                    .OrderByDescending(b => b.CreateDate) 
                    .Skip(skip)
                    .Take(take)
                    .Select(b => new BlogVM
                    {
                        Id = b.Id,
                        Title = b.Title ?? "",
                        Description = b.Description ?? "",
                        Image = b.Image ?? "default.jpg",
                        CreateDate = b.CreateDate,
                        BlogCategoryName=b.BlogCategory.Name
                        
                    })
                    .ToListAsync();

                return blogs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetBlogsAsync Error: {ex.Message}");
                return new List<BlogVM>();
            }
        }

        public async Task<int> GetTotalBlogCountAsync()
        {
            try
            {
                return await _context.Blogs.CountAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetTotalBlogCountAsync Error: {ex.Message}");
                return 0;
            }
        }
    }
}
