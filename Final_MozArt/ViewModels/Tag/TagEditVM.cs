using System.ComponentModel.DataAnnotations;

namespace Final_MozArt.ViewModels.Tag
{
    public class TagEditVM
    {
        public int Id { get; set; }

        [RegularExpression(@"^(?=.[A-Za-z])[A-Za-z0-9_:;""'\.,<>!@#$%\^&\(\)\{\}\-=\+\[\]\\|? ]*$",
        ErrorMessage = "Title must contain at least one letter and can include letters, numbers, and allowed symbols.")]
        public string Name { get; set; }
    }
}
