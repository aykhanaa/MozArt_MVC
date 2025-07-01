using Final_MozArt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Final_MozArt.Controllers
{
    public class BasketController : Controller
    {
        private readonly ISettingService _settingService;
        private readonly IBasketService _basketService;
        public BasketController(ISettingService settingService, IBasketService basketService)
        {
            _settingService = settingService;
            _basketService = basketService;

        }
        public async Task<IActionResult> Index()
        {
            return View(await _basketService.GetBasketDatasAsync());
        }



        [HttpPost]
        public async Task<IActionResult> PlusIcon(int id)
        {
            var data = await _basketService.PlusIcon(id);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> MinusIcon(int id)
        {
            var data = await _basketService.MinusIcon(id);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _basketService.DeleteItem(id);

            return Ok(data);
        }
    }
}
