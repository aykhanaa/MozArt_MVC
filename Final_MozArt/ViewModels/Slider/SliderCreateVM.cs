using System.ComponentModel.DataAnnotations;

namespace Final_MozArt.ViewModels.Slider
{
    public class SliderCreateVM
    {
        [RegularExpression(@"^(?=.[A-Za-z])[A-Za-z0-9_:;""'\.,<>!@#$%\^&\(\)\{\}\-=\+\[\]\\|? ]*$",
        ErrorMessage = "Title must contain at least one letter and can include letters, numbers, and allowed symbols.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please upload an image.")]
        public IFormFile Photo { get; set; }
    }
}
 

//public class BlogCreateVM
//{
//    [Required(ErrorMessage = "Title is required.")]
//    [MaxLength(60, ErrorMessage = "Title cannot exceed 60 characters.")]
//    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Title can only contain letters and spaces.")]
//    public string Title { get; set; }

//    [Required(ErrorMessage = "Description is required.")]
//    [MaxLength(90, ErrorMessage = "Description cannot exceed 90 characters.")]
//    public string Description { get; set; }

//    [Required(ErrorMessage = "Please upload an image.")]
//    [MaxFileSize(3 * 1024 * 1024)]
//    public IFormFile ImgFile { get; set; }

//    [Required(ErrorMessage = "Date is required.")]
//    public DateTime Date { get; set; } = DateTime.Now;
//}