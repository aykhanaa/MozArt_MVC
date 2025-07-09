using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Account;
using Final_MozArt.ViewModels.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Final_MozArt.Controllers
{
    public class MyAccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ISettingService _settingService;
        private readonly IAccountService _accountService;
        private readonly IOrderService _orderService;

        public MyAccountController(UserManager<AppUser> userManager, ISettingService settingService, 
                                   IAccountService accountService, IOrderService orderService)
        {
            _settingService = settingService;
            _userManager = userManager;
            _accountService = accountService;
            _orderService = orderService;
        }
        public async Task<IActionResult> Index()
        {
            var setting = _settingService.GetSettings();
            var userId = _userManager.GetUserId(User); 
            var user = await _accountService.GetUserByIdAsync(userId); 
            var orders = await _orderService.GetAllAsync();



            MyAccountVM model = new MyAccountVM()
            {
                Setting = setting,
                AppUser = user ,
                Orders = orders
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEmail(string newEmail)
        {
            var userId = _userManager.GetUserId(User);
            var result = await _accountService.UpdateEmailAsync(userId, newEmail);
            return Json(new { message = result });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUsername(string newUsername)
        {
            var userId = _userManager.GetUserId(User);
            var result = await _accountService.UpdateUsernameAsync(userId, newUsername);
            return Json(new { message = result });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "User ID cannot be null or empty." });
            }

            try
            {
                var user = await _accountService.GetUserByIdAsync(userId);

                var userData = new
                {
                    user.Id,
                    user.FullName,
                    user.Email,
                    user.UserName
                };

                return Json(userData);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

    }
}
