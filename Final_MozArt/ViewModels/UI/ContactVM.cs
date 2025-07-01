using Final_MozArt.ViewModels.ContactIntro;
using Final_MozArt.ViewModels.ContactMessage;


namespace Final_MozArt.ViewModels.UI
{
    public class ContactVM
    {
        public IEnumerable<ContactIntroVM> ContactIntros { get; set; }
        public Dictionary<string,string> Setting {  get; set; }
    }
}
