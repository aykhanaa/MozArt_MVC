namespace Final_MozArt.ViewModels.ProductComment
{
    public class ProductCommentVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? AppUserId { get; set; }
        public int ProductId { get; set; }
    }
}
