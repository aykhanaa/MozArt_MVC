using Final_MozArt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Final_MozArt.ViewComponents.Footer
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;

        public FooterViewComponent(ISettingService settingService)
        {

            _settingService = settingService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var setting =  _settingService.GetSettings();
            return await Task.FromResult(View(setting));
        }
    }
}
