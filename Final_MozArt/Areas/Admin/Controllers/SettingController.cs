using AutoMapper;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Setting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_MozArt.Areas.Admin.Controllers
{
    public class SettingController : MainController
    {
        private readonly ISettingService _settingService;
        private readonly IMapper _mapper;

        public SettingController(ISettingService settingService, IMapper mapper)
        {
            _settingService = settingService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Index()
        {
            var settings = await _settingService.GetAllAsync();
            return View(settings);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Error");

            var setting = await _settingService.GetByIdAsync(id.Value);
            if (setting == null)
                return RedirectToAction("Index", "Error");

            var vm = _mapper.Map<SettingVM>(setting);
            return View(vm);
        }


        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var setting = await _settingService.GetByIdAsync(id.Value);
            if (setting == null) return NotFound();

            var vm = new SettingEditVM
            {
                Id = setting.Id,
                Key = setting.Key,
                Value = setting.Value
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Edit(int id, SettingEditVM vm)
        {
            if (id != vm.Id) return BadRequest();

            if (!ModelState.IsValid) return View(vm);

            try
            {
                await _settingService.EditAsync(vm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
