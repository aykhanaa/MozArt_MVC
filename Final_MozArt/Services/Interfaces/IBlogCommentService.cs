using Final_MozArt.ViewModels.BlogComment;

namespace Final_MozArt.Services.Interfaces
{
    public interface IBlogCommentService
    {
        Task<bool> CreateAsync(BlogCommentCreateVM commentVM, string userId);
        Task<List<BlogCommentVM>> GetAllByBlogIdAsync(int blogId);
        Task DeleteAsync(int id);
        Task<BlogCommentVM> GetByIdAsync(int id);
        Task<List<BlogCommentVM>> GetAllAsync();
    }
}
