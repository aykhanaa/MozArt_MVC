using Final_MozArt.ViewModels.BlogComment;

namespace Final_MozArt.ViewModels.Blog
{
    public class BlogDetailVM
    {
        public List<BlogVM> Blogs { get; set; }
        public BlogVM Blog { get; set; }
        public Dictionary<string, string> Setting { get; set; }
        public List<BlogCommentVM> BlogComments { get; set; }

    }
}
