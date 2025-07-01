using Final_MozArt.Services;
using Final_MozArt.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_MozArt.Areas.Admin.Controllers
{
    public class ContactMessageController : MainController
    {
        private readonly IContactMessageService _contactMessageService;
        public ContactMessageController(IContactMessageService contactMessageService)
        {
            _contactMessageService = contactMessageService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]

        public async Task<IActionResult> Index()
        {
            var contactMessages = await _contactMessageService.GetAllMessagesAsync();
            return View(contactMessages);
        }

         [HttpPost]
         [Authorize(Roles = "SuperAdmin")]
         public async Task<IActionResult> Approve(int id)
         {
            await _contactMessageService.ApproveMessageAsync(id);
            return Json(new { success = true });
         }

      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize(Roles = "SuperAdmin")]
      public async Task<IActionResult> Delete(int id)
      {
          await _contactMessageService.DeleteAsync(id);
          return Json(new { success = true });
      }

    }
 }
