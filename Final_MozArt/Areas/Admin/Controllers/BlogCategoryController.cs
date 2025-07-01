using AutoMapper;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.BlogCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Final_MozArt.Areas.Admin.Controllers
{
    public class BlogCategoryController : MainController
    {
        private readonly IBlogCategoryService _blogCategoryService;
        private readonly IMapper _mapper;

        public BlogCategoryController(IBlogCategoryService blogCategoryService, IMapper mapper)
        {
            _blogCategoryService = blogCategoryService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            var categories = await _blogCategoryService.GetAllAsync();
            return View(categories);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Error");

            var category = await _blogCategoryService.GetByIdAsync(id.Value);
            if (category == null)
                return RedirectToAction("Index", "Error");

            return View(category);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Error");

            var category = await _blogCategoryService.GetEntityByIdAsync(id.Value);
            if (category == null)
                return NotFound();

            var model = new BlogCategoryEditVM
            {
                Id = category.Id,
                Name = category.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int id, BlogCategoryEditVM request)
        {
            if (id != request.Id) return BadRequest();

            if (!ModelState.IsValid) return View(request);

            await _blogCategoryService.EditAsync(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _blogCategoryService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create(BlogCategoryCreateVM request)
        {
            if (!ModelState.IsValid)
                return View(request);

            await _blogCategoryService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }
    }
}
