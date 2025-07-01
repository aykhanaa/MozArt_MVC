namespace Final_MozArt.ViewModels.Blog
{
    public class BlogVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }
        public string Banner { get; set; }

        public int BlogCategoryId { get; set; }

        public string BlogCategoryName { get; set; } 
        

        public DateTime CreateDate { get; set; }
    }
}
