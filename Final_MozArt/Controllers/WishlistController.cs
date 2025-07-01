using Final_MozArt.Services;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Shop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Final_MozArt.Controllers
{
    public class WishlistController : Controller
    {
        private readonly IWishlistService _wishlistService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductService _productService;

        public WishlistController(IWishlistService wishlistService, IHttpContextAccessor httpContextAccessor, IProductService productService)
        {
            _wishlistService = wishlistService;
            _httpContextAccessor = httpContextAccessor;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            // return View(await _wishlistService.GetWishlistDatasAsync());
            List<WishlistVM> wishlist = new();

            if (Request.Cookies["wishlist"] != null)
            {
                wishlist = JsonConvert.DeserializeObject<List<WishlistVM>>(Request.Cookies["wishlist"]);
            }

            var ids = wishlist.Select(x => x.Id).ToList();

            var products = await _productService.GetProductsByIdsAsync(ids);

            // WishlistDetailVM-ə map et
            var wishlistDetails = products.Select(p => new WishlistDetailVM
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Image = p.Image
            }).ToList();

            return View(wishlistDetails);
            // var wishlist = HttpContext.Session.Get<List<WishlistDetailVM>>("wishlist") ?? new List<WishlistDetailVM>();
            //return View(wishlist);
        }

        //[HttpPost]
        //public IActionResult Delete(int id)
        //{
        //    _wishlistService.DeleteItem(id);
        //    List<WishlistVM> wishlist = _wishlistService.GetDatasFromCookies();

        //    return Ok(wishlist.Count);
        //}
        //[HttpPost]
        //public IActionResult Delete(int id)
        //{
        //    if (_httpContextAccessor.HttpContext == null)
        //    {
        //        return BadRequest("HttpContext mövcud deyil.");
        //    }

        //    var session = _httpContextAccessor.HttpContext.Session;

        //    var wishlist = session.Get<List<WishlistDetailVM>>("wishlist") ?? new List<WishlistDetailVM>();

        //    var item = wishlist.FirstOrDefault(x => x.Id == id);
        //    if (item != null)
        //    {
        //        wishlist.Remove(item);
        //        session.Set("wishlist", wishlist);
        //    }

        //    return Ok(wishlist.Count);
        //}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return BadRequest("HttpContext mövcud deyil.");
            }

            var session = _httpContextAccessor.HttpContext.Session;
            var context = _httpContextAccessor.HttpContext;

            // Session-dan sil
            var wishlistSession = session.Get<List<WishlistDetailVM>>("wishlist") ?? new List<WishlistDetailVM>();
            var itemSession = wishlistSession.FirstOrDefault(x => x.Id == id);
            if (itemSession != null)
            {
                wishlistSession.Remove(itemSession);
                session.Set("wishlist", wishlistSession);
            }

            // 🍪 Cookie-dən də sil
            var wishlistCookieStr = context.Request.Cookies["wishlist"];
            if (wishlistCookieStr != null)
            {
                var wishlistCookie = JsonConvert.DeserializeObject<List<WishlistVM>>(wishlistCookieStr);
                var itemCookie = wishlistCookie.FirstOrDefault(x => x.Id == id);
                if (itemCookie != null)
                {
                    wishlistCookie.Remove(itemCookie);
                    context.Response.Cookies.Append("wishlist", JsonConvert.SerializeObject(wishlistCookie));
                }
            }

            return Ok(wishlistSession.Count);
        }


    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            var json = JsonConvert.SerializeObject(value);
            session.SetString(key, json);
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}