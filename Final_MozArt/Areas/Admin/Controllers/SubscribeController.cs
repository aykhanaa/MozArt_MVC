using Final_MozArt.Services;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Subscribe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_MozArt.Areas.Admin.Controllers
{
    public class SubscribeController : MainController
    {
        private readonly ISubscribeService _subscribeService;

        public SubscribeController(ISubscribeService subscribeService)
        {
            _subscribeService = subscribeService;
        }

        public async Task<IActionResult> Index()
        {
            var newsletters = await _subscribeService.GetAllAsync();

            var viewModel = new SubscribeListVM
            {
                Items = newsletters.Select(n => new SubscribeVM
                {
                    Id = n.Id,
                    Email = n.Email,
                    CreateDate = n.CreateDate
                }).ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _subscribeService.DeleteAsync(id);
            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe([FromForm] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Json(new { success = false, message = "Email is required." });
            }

            if (await _subscribeService.CheckEmailExistsAsync(email))
            {
                return Json(new { success = false, message = "This email is already subscribed." });
            }

            await _subscribeService.AddAsync(email);
            return Json(new { success = true, message = "You have successfully subscribed!" });
        }

    }
}

