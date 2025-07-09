using Final_MozArt.Models;
using Final_MozArt.ViewModels.Order;
using System.ComponentModel.DataAnnotations;

namespace Final_MozArt.ViewModels.UI
{
    public class MyAccountVM
    {
        public string UserId { get; set; }
        public Dictionary<string, string> Setting { get; set; }
        public string UserFullName { get; set; }
        public AppUser AppUser { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public List<OrderVM> Orders { get; set; }

    }
}
