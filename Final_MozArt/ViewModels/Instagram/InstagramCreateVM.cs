using System.ComponentModel.DataAnnotations;

namespace Final_MozArt.ViewModels.Instagram
{
    public class InstagramCreateVM
    {
        [Required(ErrorMessage = "Please upload an image.")]
        public IFormFile Photo { get; set; }
    }
}
