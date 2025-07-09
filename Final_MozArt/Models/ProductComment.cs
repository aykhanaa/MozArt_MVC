namespace Final_MozArt.Models
{
    public class ProductComment : BaseEntity
    {   public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
