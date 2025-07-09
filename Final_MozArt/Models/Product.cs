namespace Final_MozArt.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public ICollection<ProductImage> Images { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public ICollection<ProductColor> ProductColors { get; set; }
        public ICollection<ProductTag> ProductTags { get; set; }
        public List<BasketProduct> BasketProducts { get; set; }
        public List<WishlistProduct> WishlistProducts { get; set; }
        public List<ProductComment> ProductComments { get; set; }
        public List<OrderItem> OrderItems { get; set; }


    }
}
