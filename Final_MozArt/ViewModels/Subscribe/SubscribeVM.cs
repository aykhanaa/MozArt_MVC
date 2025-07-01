using System.ComponentModel.DataAnnotations;

namespace Final_MozArt.ViewModels.Subscribe
{
    public class SubscribeVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        public DateTime CreateDate { get; set; }

    }
}
