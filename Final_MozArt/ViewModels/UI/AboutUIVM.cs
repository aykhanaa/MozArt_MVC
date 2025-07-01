using Final_MozArt.ViewModels.About;
using Final_MozArt.ViewModels.Advantage;
using Final_MozArt.ViewModels.Instagram;
using Final_MozArt.ViewModels.Slider;
using Final_MozArt.ViewModels.Support;
using Final_MozArt.ViewModels.Video;

namespace Final_MozArt.ViewModels.UI
{
    public class AboutUIVM
    {
        public IEnumerable<AboutVM> Abouts { get; set; }
        public IEnumerable<SupportVM> Supports { get; set; }
        public IEnumerable<InstagramVM> Instagrams { get; set; }
        public Dictionary<string, string> Setting {  get; set; }
        public IEnumerable<VideoVM>  Videos { get; set; }
        public IEnumerable<AdvantageVM> Advantages { get; set; }



    }
}
