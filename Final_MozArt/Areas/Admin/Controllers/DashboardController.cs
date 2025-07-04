using Final_MozArt.Models;
using Final_MozArt.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_MozArt.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize(Roles = "SuperAdmin, Admin")]
    public class DashboardController : MainController
    {
        private readonly IProductService _productService;
        public DashboardController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index(string text)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            IEnumerable<Product> products = Enumerable.Empty<Product>();
            if (!string.IsNullOrWhiteSpace(text)) 
            { 
              products = await _productService.SearchProductsAsync(text);
            }
            return View(products);
        }
    }
}
