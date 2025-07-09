namespace Final_MozArt.ViewModels.Order
{
    public class OrderVM
    {
        public int Id { get; set; }
        public string AppUserEmail { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemVM> Items { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    
}
