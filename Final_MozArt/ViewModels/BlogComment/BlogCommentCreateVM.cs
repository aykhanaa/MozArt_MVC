using System.ComponentModel.DataAnnotations;

namespace Final_MozArt.ViewModels.BlogComment
{
    public class BlogCommentCreateVM
    {
        [Required]
        public int BlogId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Comment { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
