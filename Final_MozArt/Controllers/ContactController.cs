    using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.ContactMessage;
using Final_MozArt.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;

namespace Final_MozArt.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactIntroService _contactIntroService;
        private readonly IContactMessageService _contactMessageService;
        private readonly ISettingService _settingService;

        public ContactController(IContactIntroService contactIntroService, IContactMessageService contactMessageService, ISettingService settingService)
        {
            _contactIntroService = contactIntroService;
            _contactMessageService = contactMessageService;
            _settingService = settingService;
        }


        public async Task<IActionResult> Index()
        {
            var contactintros = await _contactIntroService.GetAllAsync();
            var setting = _settingService.GetSettings();
            

            ContactVM model = new ContactVM()
            {
                ContactIntros = contactintros,
                Setting = setting
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] string email, [FromForm] string name, [FromForm] string message)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Json(new { success = false, message = "Email is required." });

            try
            {
                var dto = new ContactMessageCreateVM
                {
                    Name = name,
                    Email = email,
                    Message = message
                };

                var success = await _contactMessageService.CreateAsync(dto);

                if (success)
                    return Json(new { success = true, message = "Your message has been sent successfully." });
                else
                    return Json(new { success = false, message = "Please register first before sending a message." });
            }
            catch
            {
                return Json(new { success = false, message = "An unexpected error occurred." });
            }
        }

    }
}
