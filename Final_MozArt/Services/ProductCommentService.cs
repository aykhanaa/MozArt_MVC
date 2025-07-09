using AutoMapper;
using Final_MozArt.Data;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.BlogComment;
using Final_MozArt.ViewModels.ProductComment;
using Microsoft.EntityFrameworkCore;

namespace Final_MozArt.Services
{
    public class ProductCommentService : IProductCommentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductCommentService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(ProductCommentCreateVM commentVM, string userId)
        {
            if (commentVM == null)
                return false;

            var comment = _mapper.Map<ProductComment>(commentVM);
            comment.AppUserId = userId;
            comment.CreateDate = DateTime.UtcNow;

            await _context.ProductComments.AddAsync(comment);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task DeleteAsync(int id)
        {
            var comment = await _context.ProductComments.FindAsync(id);
            if (comment != null)
            {
                _context.ProductComments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ProductCommentVM>> GetAllAsync()
        {
            return _mapper.Map<List<ProductCommentVM>>(await _context.ProductComments .ToListAsync());
        }

        public async Task<List<ProductCommentVM>> GetAllByProductIdAsync(int productId)
        {
            var comments = await _context.ProductComments
           .AsNoTracking()
           .Where(c => c.ProductId == productId)
           .OrderByDescending(c => c.CreateDate)
           .ToListAsync();

            return _mapper.Map<List<ProductCommentVM>>(comments);
        }

        public async Task<ProductCommentVM> GetByIdAsync(int id)
        {
            var comment = await _context.ProductComments
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            return _mapper.Map<ProductCommentVM>(comment);
        }
    }
}
