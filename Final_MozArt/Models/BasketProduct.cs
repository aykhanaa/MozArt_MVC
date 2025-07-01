namespace Final_MozArt.Models
{
    public class BasketProduct :BaseEntity
    {
        public int Count { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int BasketId { get; set; }
        public Basket Basket { get; set; }
    }
}
