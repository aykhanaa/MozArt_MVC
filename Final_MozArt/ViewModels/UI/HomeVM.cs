using Final_MozArt.ViewModels.Brand;
using Final_MozArt.ViewModels.Category;
using Final_MozArt.ViewModels.ContactMessage;
using Final_MozArt.ViewModels.Instagram;
using Final_MozArt.ViewModels.Product;
using Final_MozArt.ViewModels.Slider;

namespace Final_MozArt.ViewModels.UI
{
    public class HomeVM
    {
        public IEnumerable<SliderVM> Sliders { get; set; }
        public IEnumerable<CategoryVM> Categories { get; set; }
        public IEnumerable<BrandVM> Brands { get; set; }
        public IEnumerable<InstagramVM> Instagrams { get; set; }
        public IEnumerable<ProductVM> Products { get; set; }
        public IEnumerable<ContactMessageVM> ApprovedMessages { get; set; }
        public Dictionary<string, string> Setting { get; set; }
        public bool HasResults { get; set; }
    }
}
