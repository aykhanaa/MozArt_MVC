using System.ComponentModel.DataAnnotations;

namespace Final_MozArt.ViewModels.Instagram
{
    public class InstagramEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please upload an image.")]
        public IFormFile? Photo { get; set; }

        public string Image { get; set; }
    }
}
