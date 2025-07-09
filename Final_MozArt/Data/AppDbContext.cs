using Final_MozArt.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace Final_MozArt.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
       
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Instagram> Instagrams { get; set; }
        public DbSet<Support> Supports { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Advantage> Advantages { get; set; }
        public DbSet<ContactIntro> ContactIntros { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProductColor>  ProductColors { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProductTag>  ProductTags { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketProduct> BasketProducts { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistProduct> WishlistProducts { get; set; }
        public DbSet<ProductComment>  ProductComments { get; set; }
        public DbSet<Order>  Orders { get; set; }
        public DbSet<OrderItem>  OrderItems { get; set; }
    }
}
