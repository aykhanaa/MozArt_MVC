using Final_MozArt.Models;

namespace Final_MozArt.ViewModels.Order
{
    public class OrderVM
    {
        public int Id { get; set; }
        public string AppUserEmail { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemVM> Items { get; set; }
        public DateTime CreatedDate { get; set; }

        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }

        public bool? Status { get; set; }
        public bool IsCanceled { get; set; }
    }
    
}
