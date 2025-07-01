using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Layout;

namespace Final_MozArt.Services
{
    public class LayoutService : ILayoutService
    {
        private readonly IBasketService _basketService;
        private readonly ISettingService _settingService;
        private readonly IWishlistService _wishlistService;

        public LayoutService(IBasketService basketService, ISettingService settingService, IWishlistService wishlistService)
        {
            _basketService = basketService;
            _settingService = settingService;
            _wishlistService = wishlistService;
            
        }
        public HeaderVM GetHeaderDatas()
        {
            Dictionary<string, string> settingDatas = _settingService.GetSettings();
            int basketCount = _basketService.GetCount();
            int wishlistCount = _wishlistService.GetCount();

          
            return new HeaderVM
            {
                BasketCount = basketCount,
                WishlistCount = wishlistCount,
                Setting = settingDatas 
            };
        }
    }
}
