using Final_MozArt.Models;
using Final_MozArt.ViewModels.Blog;
using Final_MozArt.ViewModels.Product;



public interface IProductService
{
    Task<ICollection<ProductVM>> GetAllAsync();
    Task<ProductVM> GetByIdWithIncludesAsync(int id);
    Task<ProductDetailVM> GetByIdWithIncludesWithoutTrackingAsync(int id);
    Task<ICollection<ProductVM>> GetPaginatedDatasByCategory(int id, int page, int take);
    Task<ICollection<ProductVM>> GetPaginatedDatasAsync(int page, int take);
    Task<ProductVM> GetByNameWithoutTrackingAsync(string name);
    Task<int> GetCountAsync();
    Task<int> GetCountByCategoryAsync(int id);
    Task<int> GetCountBySearch(string searchText);
    Task<ICollection<ProductVM>> SearchAsync(string searchText, int page, int take);
    Task CreateAsync(ProductCreateVM product);
    Task EditAsync(ProductEditVM product);
    Task DeleteAsync(int id);
    Task DeleteProductImageAsync(int id);
    Task SetMainImageAsync(SetIsMainVM data);
    Task<List<Product>> SearchProductsAsync(string query);
    Task<ICollection<ProductVM>> SortAsync(string sortKey);
    Task<ICollection<ProductVM>> FilterAsync(string? categoryName, string? brandName, string? tagName);
    Task<List<ProductVM>> GetProductAsync(int skip, int take);
    Task<int> GetTotalProductCountAsync();
    Task<List<ProductVM>> FilterByPriceAsync(decimal min, decimal max);
    Task<List<ProductVM>> GetProductsByIdsAsync(List<int> ids);

}


