namespace Final_MozArt.Models
{
    public class BlogCategory:BaseEntity
    {
        public string Name { get; set; }
        ICollection<Blog> Blogs { get; set; }

    }
}
