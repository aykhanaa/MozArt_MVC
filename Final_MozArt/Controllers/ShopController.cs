using Final_MozArt.Services.Implementations;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.BlogComment;
using Final_MozArt.ViewModels.Product;
using Final_MozArt.ViewModels.ProductComment;
using Final_MozArt.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;


namespace Final_MozArt.Controllers
{
    public class ShopController : Controller
    {
        private readonly ISettingService _settingService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private  readonly ITagService _tagService;
        private readonly IBasketService _basketService;
        private readonly IWishlistService _wishlistService; 
        private readonly IProductCommentService _productCommentService;



        public ShopController(ISettingService settingService,
                              IProductService productService,
                              ICategoryService categoryService,
                              IBrandService brandService,
                              ITagService tagService,
                              IBasketService basketService,
                              IWishlistService wishlistService,
                              IProductCommentService productCommentService)
        {
            _settingService = settingService;
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _tagService = tagService;
            _basketService = basketService;
            _wishlistService = wishlistService;
            _productCommentService = productCommentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? sortKey, string? categoryName, string? brandName, string? tagName, string? query)
        {
            var setting = _settingService.GetSettings();
            var allProducts = await _productService.GetAllAsync();
            ICollection<ProductVM> products;
            query = query?.Trim();
            categoryName = categoryName?.Trim();
            brandName = brandName?.Trim();
            tagName = tagName?.Trim();

            if (!string.IsNullOrWhiteSpace(query))
            {
                products = allProducts
                    .Where(p => (p.Name != null && p.Name.ToLower().Contains(query.ToLower())) ||
                                (p.CategoryName != null && p.CategoryName.ToLower().Contains(query.ToLower())))
                    .ToList();

                if (products == null || products.Count == 0)
                {
                    return RedirectToAction("Index", "NotFound");
                }
            }
            else if (!string.IsNullOrWhiteSpace(sortKey))
            {
                products = await _productService.SortAsync(sortKey);
            }
            else if (!string.IsNullOrWhiteSpace(categoryName) || !string.IsNullOrWhiteSpace(brandName) || !string.IsNullOrWhiteSpace(tagName))
            {
                // FilterAsync metoduna original format-da göndər
                products = await _productService.FilterAsync(categoryName, brandName, tagName);

            }
            else
            {
                products = allProducts;
            }

            var categories = await _categoryService.GetAllAsync();
            var categoryiesWithProductCount = await _categoryService.GetProductCountByCategoryNameAsync();
            var brandWithProductCount = await _brandService.GetProductCountByBrandNameAsync();
            var brands = await _brandService.GetAllAsync();
            var tags = await _tagService.GetAllAsync();

            var model = new ShopVM
            {
                Setting = setting,
                Products = products,
                Categories = categories,
                ProductCount = categoryiesWithProductCount,
                Brands = brands,
                Tags = tags,
                BrandsProductCount = brandWithProductCount,
                AllProducts = allProducts
            };

            return View(model);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var model = await _productService.GetByIdWithIncludesWithoutTrackingAsync(id);
            if (model == null) return RedirectToAction("Index","NotFound");


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetSortedProducts(string? sortKey)
        {
            ICollection<ProductVM> products;

            if (string.IsNullOrWhiteSpace(sortKey) || sortKey == "default")
                products = await _productService.GetAllAsync();
            else
                products = await _productService.SortAsync(sortKey);

            var result = products.Select(p => new
            {
                id = p.Id,
                name = p.Name,
                price = p.Price,
                image = p.Image,
                hower = p.Hower
            });

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredProducts(string? categoryName, string? brandName, string? tagName)
        {
            var products = await _productService.FilterAsync(categoryName, brandName, tagName);

            var result = products.Select(p => new
            {
                id = p.Id,
                name = p.Name,
                price = p.Price,
                image = p.Image,
                hower = p.Hower
            });

            return Json(result);
        }
        [HttpGet]
        public async Task<IActionResult> FilterByPrice(decimal min, decimal max)
        {
            var products = await _productService.FilterByPriceAsync(min, max);

            return Json(products);
        }

      

        public async Task<IActionResult> LoadMore(int skip = 0, int take = 3)
        {
            try
            {
                var products = await _productService.GetProductAsync(skip, take);

                var formattedProducts = products.Select(p => new
                {
                    id = p.Id,
                    name = p.Name,
                    description = p.Description,
                    price = p.Price,
                    image = p.Image,
                    hower = p.Hower,
                    createDate = p.CreateDate,
                    categoryName = p.CategoryName,
                    brandName = p.BrandName
                }).ToList();

                return Json(new { success = true, products = formattedProducts });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoadMore Error: {ex.Message}");
                return Json(new { success = false, message = ex.Message, products = new List<object>() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCommentCreateVM commentVM)
        {
            if (!ModelState.IsValid)
            {
                // Model validation error-larını JSON formatında qaytarırıq
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return Json(new { success = false, message = "Validation failed.", errors });
            }

            try
            {
                string userId = null;

                if (User.Identity.IsAuthenticated)
                {
                    userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                }

                var success = await _productCommentService.CreateAsync(commentVM, userId);

                if (success)
                    return Json(new { success = true, message = "Comment submitted successfully." });
                else
                    return Json(new { success = false, message = "Failed to submit comment." });
            }
            catch (Exception ex)
            {
                // Log exception burda edilə bilər (əgər log sistemi varsa)
                return Json(new { success = false, message = "An unexpected error occurred." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddBasket(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(); // 401 qaytarır
            }

            if (id is null) return RedirectToAction("Index", "Error");

            ProductDetailVM product = await _productService.GetByIdWithIncludesWithoutTrackingAsync((int)id);

            if (product is null) return RedirectToAction("Index", "Error");

            _basketService.AddBasket((int)id, product);

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> AddWishlist(int? id)
        {

            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(); 
            }

            if (id is null) return RedirectToAction("Index", "Error"); ;

            ProductVM product = await _productService.GetByIdWithIncludesAsync((int)id);

            if (product is null) return RedirectToAction("Index", "Error"); ;

            int a = _wishlistService.AddWishlist((int)id, product);

            return Ok(a);
        }

    }
}
