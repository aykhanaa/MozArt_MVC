using System.ComponentModel.DataAnnotations;

namespace Final_MozArt.ViewModels.Color
{
    public class ColorCreateVM
    {
        [RegularExpression(@"^[A-Za-z]+$", 
        ErrorMessage = "Only letters are allowed.")]

        public string Name { get; set; }
    }
}
