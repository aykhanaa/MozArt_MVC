using Final_MozArt.ViewModels.Brand;
using Final_MozArt.ViewModels.Category;
using Final_MozArt.ViewModels.Product;
using Final_MozArt.ViewModels.Tag;

namespace Final_MozArt.ViewModels.UI
{
    public class ShopVM
    {
        public Dictionary<string, string> Setting { get; set; }
        public IEnumerable<ProductVM> Products { get; set; }
        public IEnumerable<CategoryVM> Categories { get; set; }
        public IEnumerable<BrandVM> Brands { get; set; }
        public IEnumerable<TagVM> Tags { get; set; }
        public Dictionary<string, int> ProductCount { get; set; }
        public Dictionary<string, int> BrandsProductCount { get; set; }
        public ICollection<ProductVM> AllProducts { get; set; }
        public int Count { get; set; }

    }
}
