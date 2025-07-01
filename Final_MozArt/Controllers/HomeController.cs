using Final_MozArt.Models;
using Final_MozArt.Services;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Product;
using Final_MozArt.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Final_MozArt.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly IInstagramService _instagramService;
        private readonly IProductService _productService;
        private readonly IContactMessageService _contactMessageService;
        private readonly ISettingService _settingService;




        public HomeController(ISliderService sliderService, 
                              ICategoryService categoryService,
                              IBrandService brandService,
                              IInstagramService instagramService,
                              IProductService productService,
                              IContactMessageService contactMessageService,
                              ISettingService settingService
                            )
        {
            _sliderService = sliderService;
            _categoryService = categoryService;
            _brandService = brandService;
            _instagramService = instagramService;
            _productService = productService;
            _contactMessageService = contactMessageService;
            _settingService = settingService;
          
        }
        public async Task<IActionResult> Index()
        {
            var sliders = await _sliderService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();
            var brands = await _brandService.GetAllAsync();
            var instagrams = await _instagramService.GetAllAsync();
            var products = await _productService.GetAllAsync();
            var approvedMessages = await _contactMessageService.GetAllApprovedMessagesAsync();
            var setting = _settingService.GetSettings();




            HomeVM model = new HomeVM()
            {
                Sliders = sliders,
                Categories = categories,
                Brands = brands,
                Instagrams = instagrams,
                Products = products,
                ApprovedMessages = approvedMessages,
                Setting = setting

            };

            return View(model);

        }
        public async Task<IActionResult> Search(string query)
        {
            var products = await _productService.SearchProductsAsync(query); // List<Product>

            var productVMs = products.Select(p => new ProductVM
            {
                Id = p.Id,
                Name = p.Name,
                CategoryName = p.Category?.Name
            }).ToList();

            var model = new HomeVM
            {
                Products = productVMs
                                     
            };

            return View("Index", model);
        }





    }
}
