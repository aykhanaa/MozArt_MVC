using Final_MozArt.Models;
using Final_MozArt.ViewModels.Blog;


namespace Final_MozArt.Services.Interfaces
{
    public interface IBlogService
    {
        Task<List<BlogVM>> GetAllAsync();
        Task<BlogVM> GetByIdAsync(int id);
        Task<Blog> GetEntityByIdAsync(int id);
        Task CreateAsync(BlogCreateVM request);
        Task EditAsync(BlogEditVM request);
        Task DeleteAsync(int id);
        Task<List<BlogVM>> GetBlogsAsync(int skip, int take);
        Task<int> GetTotalBlogCountAsync();
    }
}
