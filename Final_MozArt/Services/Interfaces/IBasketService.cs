
using Final_MozArt.Models;
using Final_MozArt.ViewModels.Basket;
using Final_MozArt.ViewModels.Product;

namespace Final_MozArt.Services.Interfaces
{
    public interface IBasketService
    {
        void AddBasket(int id, ProductDetailVM product);
        int GetCount();
        Task<List<BasketDetailVM>> GetBasketDatasAsync();
        Task<CountPlusAndMinus> PlusIcon(int id);
        Task<CountPlusAndMinus> MinusIcon(int id);
        Task<DeleteBasketItemResponse> DeleteItem(int id);
        List<BasketVM> GetDatasFromCoockies();
        Task<Basket> GetByUserIdAsync(string userId);
        Task<List<BasketProduct>> GetAllByBasketIdAsync(int? basketId);
    }
}
