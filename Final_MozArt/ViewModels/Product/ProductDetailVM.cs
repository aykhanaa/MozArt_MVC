using Final_MozArt.Models;
using Final_MozArt.ViewModels.ProductComment;


namespace Final_MozArt.ViewModels.Product
{
    public class ProductDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int BrandId { get; set; }
        public string BrandName { get; set; }

        public ICollection<string> ColorNames { get; set; }
        public ICollection<string> TagNames { get; set; }

        public ICollection<int> ColorIds { get; set; }     
        public ICollection<int> TagIds { get; set; }       
        public ICollection<ProductImage> Images { get; set; }

        public DateTime CreateDate { get; set; }

        public Dictionary<string, string> Setting { get; set; }
        public ProductVM Product { get; set; }
        public List<ProductCommentVM>  ProductComments { get; set; }

    }

}
