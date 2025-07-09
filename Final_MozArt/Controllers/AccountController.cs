
using Final_MozArt.Data;
using Final_MozArt.Models;
using Final_MozArt.Services;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Account;
using Final_MozArt.ViewModels.Basket;
using Final_MozArt.ViewModels.Shop;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;

namespace Final_MozArt.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IBasketService _basketService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWishlistService _wishlistService;
        private readonly AppDbContext _context;
        public AccountController(IAccountService accountService, IBasketService basketService, UserManager<AppUser> userManager,AppDbContext context, IWishlistService wishlistService)
        {
            _accountService = accountService;
            _basketService = basketService;
            _userManager = userManager;
            _context = context;
            _wishlistService = wishlistService;
        }

        [HttpGet]
        public IActionResult Register()
        {            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM request)
        {

            if (!ModelState.IsValid) return View(request);

            var result = await _accountService.RegisterAsync(request);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View(request);
            }

            return RedirectToAction(nameof(VerifyEmail));
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _accountService.LoginAsync(request);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Login information is wrong.");
                return View(request);
            }

            AppUser dbUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == request.EmailOrUsername || u.UserName == request.EmailOrUsername);

            if (dbUser == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return View(request);
            }

            List<BasketVM> basket = new();
            Basket dbBasket = await _basketService.GetByUserIdAsync(dbUser.Id);

            if (dbBasket is not null)
            {
                List<BasketProduct> basketProducts = await _basketService.GetAllByBasketIdAsync(dbBasket.Id);

                foreach (var item in basketProducts)
                {
                    basket.Add(new BasketVM
                    {
                        Id = item.ProductId,
                        Count = item.Count
                    });
                }

                Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId is null || token is null)
                return RedirectToAction("Index", "Error");

            try
            {
                await _accountService.ConfirmEmailAsync(userId, token);
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool sent = await _accountService.ForgotPasswordAsync(model);

            if (!sent)
            {
                ModelState.AddModelError("Email", "User is not found.");
                return View(model);
            }

            TempData["Email"] = model.Email;
            return RedirectToAction(nameof(VerifyResetPassword));
        }

        [HttpGet]
        public IActionResult VerifyResetPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            return View(new ResetPasswordVM { UserId = userId, Token = token });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _accountService.ResetPasswordAsync(model);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Logout()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _accountService.LogoutAsync();

            List<BasketVM> basket = _basketService.GetDatasFromCoockies();
            Basket dbBasket = await _basketService.GetByUserIdAsync(userId);

            if (basket.Count != 0)
            {
                if (dbBasket == null)
                {
                    dbBasket = new()
                    {
                        AppUserId = userId,
                        BasketProducts = new List<BasketProduct>()
                    };

                    foreach (var item in basket)
                    {
                        dbBasket.BasketProducts.Add(new BasketProduct()
                        {
                            ProductId = item.Id,
                            Count = item.Count
                        });
                    }

                    await _context.Baskets.AddAsync(dbBasket);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    List<BasketProduct> basketProducts = new();

                    foreach (var item in basket)
                    {
                        basketProducts.Add(new BasketProduct()
                        {
                            ProductId = item.Id,
                            Count = item.Count
                        });
                    }

                    dbBasket.BasketProducts = basketProducts;
                    await _context.SaveChangesAsync();
                }

                Response.Cookies.Delete("basket");
            }
            else
            {
                if (dbBasket is not null)
                {
                    _context.Baskets.Remove(dbBasket);
                    await _context.SaveChangesAsync();
                }
            }

            List<WishlistVM> wishlist = _wishlistService.GetDatasFromCookies();
            Wishlist dbWishlist = await _wishlistService.GetByUserIdAsync(userId);

            if (wishlist.Count != 0)
            {
                if (dbWishlist == null)
                {
                    dbWishlist = new()
                    {
                        AppUserId = userId,
                        WishlistProducts = new List<WishlistProduct>()
                    };

                    foreach (var item in wishlist)
                    {
                        dbWishlist.WishlistProducts.Add(new WishlistProduct()
                        {
                            ProductId = item.Id
                        });
                    }

                    await _context.Wishlists.AddAsync(dbWishlist);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    List<WishlistProduct> wishlistProducts = new();

                    foreach (var item in wishlist)
                    {
                        wishlistProducts.Add(new WishlistProduct()
                        {
                            ProductId = item.Id
                        });
                    }

                    dbWishlist.WishlistProducts = wishlistProducts;
                    await _context.SaveChangesAsync();
                }

                Response.Cookies.Delete("wishlist");
            }
            else
            {
                if (dbWishlist is not null)
                {
                    _context.Wishlists.Remove(dbWishlist);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized(new { Message = "User not authenticated." });

            var result = await _accountService.UpdateEmailAsync(userId, model.NewEmail);

            if (result.Contains("successfully"))
                return Ok(new { Message = result });

            return BadRequest(new { Message = result });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUsername([FromBody] UpdateUsernameVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized(new { Message = "User not authenticated." });

            var result = await _accountService.UpdateUsernameAsync(userId, model.NewUsername);

            if (result.Contains("successfully"))
                return Ok(new { Message = result });

            return BadRequest(new { Message = result });
        }

    }
}
