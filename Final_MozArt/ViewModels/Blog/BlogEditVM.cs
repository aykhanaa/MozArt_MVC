
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Final_MozArt.ViewModels.Blog
{
    public class BlogEditVM
    {
        public int Id { get; set; }

        [RegularExpression(@"^(?=.[A-Za-z])[A-Za-z0-9_:;""'\.,<>!@#$%\^&\(\)\{\}\-=\+\[\]\\|? ]*$",
        ErrorMessage = "Title must contain at least one letter and can include letters, numbers, and allowed symbols.")]
        [StringLength(150)]
        public string Title { get; set; }

        [RegularExpression(@"^(?=.[A-Za-z])[A-Za-z0-9_:;""'\.,<>!@#$%\^&\(\)\{\}\-=\+\[\]\\|? ]*$",
        ErrorMessage = "Description must contain at least one letter and can include letters, numbers, and allowed symbols.")]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        public int BlogCategoryId { get; set; }

        public string Image { get; set; }
        public IFormFile? Photo { get; set; } 
    }
}
