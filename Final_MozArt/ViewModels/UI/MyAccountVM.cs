using Final_MozArt.Models;

namespace Final_MozArt.ViewModels.UI
{
    public class MyAccountVM
    {
        public string UserId { get; set; }
        public Dictionary<string, string> Setting { get; set; }
        public string UserFullName { get; set; }
        public AppUser AppUser { get; set; }

    }
}
