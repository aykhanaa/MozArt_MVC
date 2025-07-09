namespace Final_MozArt.Models
{
    public class Order : BaseEntity
    {
        public string AppUserId { get; set; }
        public decimal TotalPrice { get; set; }
        public string StripeId { get; set; }
        public AppUser AppUser { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public bool? Status { get; set; }
        //public bool  { get; set; }
    }
}
