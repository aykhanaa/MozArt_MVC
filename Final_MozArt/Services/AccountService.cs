using Azure;
using Final_MozArt.Helpers.Enums;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Account;
using Final_MozArt.ViewModels.Basket;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Final_MozArt.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelper _urlHelper;
        private readonly IBasketService _basketService;

        public AccountService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IEmailService emailService,
            IHttpContextAccessor httpContextAccessor,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor,
            IBasketService basketService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;

            // Create UrlHelper for generating URLs
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _basketService = basketService;
            _basketService = basketService;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterVM request)
        {
            AppUser user = new()
            {
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.UserName
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded) return result;

            await _userManager.AddToRoleAsync(user, Roles.Member.ToString());

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            string url = _urlHelper.Action("ConfirmEmail", "Account", new
            {
                userId = user.Id,
                token
            }, _httpContextAccessor.HttpContext.Request.Scheme);

            string subject = "Welcome to MozArt";
            string emailHtml = File.ReadAllText("wwwroot/templates/register-confirm.html")
                                   .Replace("{{link}}", url)
                                   .Replace("{{fullName}}", user.FullName);

            _emailService.Send(user.Email, subject, emailHtml);

            return result;
        }

        public async Task<SignInResult> LoginAsync(LoginVM request)
        {
            AppUser dbUser = await _userManager.FindByEmailAsync(request.EmailOrUsername);

            if (dbUser is null)
            {
                dbUser = await _userManager.FindByNameAsync(request.EmailOrUsername);
            }

            if (dbUser is null)
            {
                return SignInResult.Failed;
            }

            var result = await _signInManager.PasswordSignInAsync(dbUser, request.Password, request.IsRememberMe, false);
            //List<BasketVM> basket = new();
            //Basket dbBasket = await _basketService.GetByUserIdAsync(dbUser.Id);

            //if (dbBasket is not null)
            //{
            //    List<BasketProduct> basketProducts = await _basketService.GetAllByBasketIdAsync(dbBasket.Id);

            //    foreach (var item in basketProducts)
            //    {
            //        basket.Add(new BasketVM
            //        {
            //            Id = item.ProductId,
            //            Count = item.Count
            //        });
            //    }

            //    Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));

            //}
            return result;
        }

        public async Task ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) throw new ArgumentException("User not found");

            await _userManager.ConfirmEmailAsync(user, token);
            await _signInManager.SignInAsync(user, false);
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordVM model)
        {
            var existUser = await _userManager.FindByEmailAsync(model.Email);

            if (existUser is null || !existUser.EmailConfirmed)
            {
                return false;
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(existUser);

            string link = _urlHelper.Action("ResetPassword", "Account", new
            {
                userId = existUser.Id,
                token
            }, _httpContextAccessor.HttpContext.Request.Scheme);

            string subject = "Reset Password";
            string html = File.ReadAllText("wwwroot/templates/reset-password.html");

            html = html.Replace("{{fullName}}", existUser.FullName)
                       .Replace("{{link}}", link);

            _emailService.Send(existUser.Email, subject, html);

            return true;
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordVM model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user is null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var passwordHasher = new PasswordHasher<AppUser>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Success)
            {
                return IdentityResult.Failed(new IdentityError { Description = "New password cannot be the same as the old password." });
            }

            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return IdentityResult.Failed(new IdentityError { Description = "New password can't be same as old password." });
            }

            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return IdentityResult.Failed(new IdentityError { Description = "New password can't be same as old password" });
            }

            return await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

    }
}
