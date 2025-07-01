using System.ComponentModel.DataAnnotations;

namespace Final_MozArt.ViewModels.About
{
    public class AboutEditVM
    {
        public int Id { get; set; }

        [RegularExpression(@"^(?=.[A-Za-z])[A-Za-z0-9_:;""'\.,<>!@#$%\^&\(\)\{\}\-=\+\[\]\\|? ]*$",
        ErrorMessage = "Title must contain at least one letter and can include letters, numbers, and allowed symbols.")]
        public string Title { get; set; }

        [RegularExpression(@"^(?=.[A-Za-z])[A-Za-z0-9_:;""'\.,<>!@#$%\^&\(\)\{\}\-=\+\[\]\\|? ]*$",
        ErrorMessage = "Subtitle must contain at least one letter and can include letters, numbers, and allowed symbols.")]
        public string Subtitle { get; set; }

        [RegularExpression(@"^(?=.[A-Za-z])[A-Za-z0-9_:;""'\.,<>!@#$%\^&\(\)\{\}\-=\+\[\]\\|? ]*$",
        ErrorMessage = "Description must contain at least one letter and can include letters, numbers, and allowed symbols.")]
        public string Description { get; set; }
        public IFormFile? Photo { get; set; }

        public string Image { get; set; }
    }
}
