using System.ComponentModel.DataAnnotations;

namespace Final_MozArt.ViewModels.Video
{
    public class VideoEditVM
    {
        public int Id { get; set; }

        [Required]
        public string VideoURL { get; set; }

        public IFormFile? Photo { get; set; }

        public string Image { get; set; }
    }
}
