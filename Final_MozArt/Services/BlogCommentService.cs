using AutoMapper;
using Final_MozArt.Data;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.BlogComment;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Final_MozArt.Services.Implementations
{
    public class BlogCommentService : IBlogCommentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BlogCommentService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(BlogCommentCreateVM commentVM, string userId)
        {
            if (commentVM == null)
                return false;

            var comment = _mapper.Map<BlogComment>(commentVM);
            comment.AppUserId = userId;
            comment.CreateDate = DateTime.UtcNow;

            await _context.BlogComments.AddAsync(comment);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<List<BlogCommentVM>> GetAllByBlogIdAsync(int blogId)
        {
            var comments = await _context.BlogComments
                .AsNoTracking()
                .Where(c => c.BlogId == blogId)
                .OrderByDescending(c => c.CreateDate)
                .ToListAsync();

            return _mapper.Map<List<BlogCommentVM>>(comments);
        }

        public async Task DeleteAsync(int id)
        {
            var comment = await _context.BlogComments.FindAsync(id);
            if (comment != null)
            {
                _context.BlogComments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<BlogCommentVM> GetByIdAsync(int id)
        {
            var comment = await _context.BlogComments
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            return _mapper.Map<BlogCommentVM>(comment);
        }

        public async Task<List<BlogCommentVM>> GetAllAsync()
        {
            return _mapper.Map<List<BlogCommentVM>>(await _context.BlogComments.ToListAsync());
        }
    }
}
