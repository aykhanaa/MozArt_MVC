
using Final_MozArt.ViewModels.ProductComment;

namespace Final_MozArt.Services.Interfaces
{
    public interface IProductCommentService
    {
        Task<bool> CreateAsync(ProductCommentCreateVM commentVM, string userId);
        Task<List<ProductCommentVM>> GetAllByProductIdAsync(int productId);
        Task DeleteAsync(int id);
        Task<ProductCommentVM> GetByIdAsync(int id);
        Task<List<ProductCommentVM>> GetAllAsync();
    }
}
