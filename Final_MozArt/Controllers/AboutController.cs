using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.About;
using Final_MozArt.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;

namespace Final_MozArt.Controllers
{
    public class AboutController : Controller
    {
        private readonly IAboutService _aboutService;
        private readonly ISupportService _supportService;
        private readonly IInstagramService _instagramService;
        private readonly ISettingService _settingService;
        private readonly IVideoService _videoService;
        private readonly IAdvantageService _advantageService;


        public AboutController(IAboutService aboutService,
                               ISupportService supportService,
                               IInstagramService ınstagramService,
                               ISettingService settingService,
                               IVideoService videoService,
                               IAdvantageService advantageService)

        {
            _aboutService = aboutService;
            _supportService = supportService;
            _instagramService = ınstagramService;
            _settingService = settingService;
            _videoService = videoService;
            _advantageService = advantageService;



        }
        public async Task<IActionResult> Index()
        {
            var abouts = await _aboutService.GetAllAsync();
            var supports = await _supportService.GetAllAsync();
            var instagrams = await _instagramService.GetAllAsync();
            var setting = _settingService.GetSettings();
            var videos = await _videoService.GetAllAsync();
            var advantages = await _advantageService.GetAllAsync();




            AboutUIVM model = new AboutUIVM()
            {
                Abouts = abouts,
                Supports = supports,
                Instagrams= instagrams,
                Setting = setting,
                Videos= videos,
                Advantages= advantages,


            };



            return View(model);
        }
    }
}
