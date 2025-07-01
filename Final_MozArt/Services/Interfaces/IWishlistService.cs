using Final_MozArt.Models;
using Final_MozArt.ViewModels.Product;
using Final_MozArt.ViewModels.Shop;

namespace Final_MozArt.Services.Interfaces
{
    public interface IWishlistService
    {
        int AddWishlist(int id, ProductVM product);
        int GetCount();
        Task<List<WishlistDetailVM>> GetWishlistDatasAsync();
        void DeleteItem(int id);
        List<WishlistVM> GetDatasFromCookies();
        void SetDatasToCookies(List<WishlistVM> wishlist, Product dbProduct, WishlistVM existProduct);
        Task<Wishlist> GetByUserIdAsync(string userId);
        Task<List<WishlistProduct>> GetAllByWishlistIdAsync(int? wishlistId);
    }
}
