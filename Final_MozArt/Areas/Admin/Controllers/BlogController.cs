using AutoMapper;
using Final_MozArt.Helpers.Extensions;
using Final_MozArt.Services;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Final_MozArt.Areas.Admin.Controllers
{
    public class BlogController : MainController
    {
        private readonly IBlogService _blogService;
        private readonly IBlogCategoryService _categoryService;
        private readonly IMapper _mapper;

        public BlogController(IBlogService blogService, IBlogCategoryService categoryService, IMapper mapper)
        {
            _blogService = blogService;
            _categoryService = categoryService;
            _mapper = mapper;
        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            var blogs = await _blogService.GetAllAsync();
            return View(blogs);
        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Error");

            var blog = await _blogService.GetByIdAsync(id.Value);
            if (blog == null) return RedirectToAction("Index", "Error");

            return View(blog);
        }


        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create(BlogCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(request);
            }

            if (!request.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Only image files are allowed");
                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(request);
            }

            if (!request.Photo.CheckFileSize(2048))
            {
                ModelState.AddModelError("Photo", "Max file size is 2MB");
                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(request);
            }

            await _blogService.CreateAsync(request);
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Error");

            var blog = await _blogService.GetEntityByIdAsync(id.Value);
            if (blog == null) return NotFound();

            var model = new BlogEditVM
            {
                Id = blog.Id,
                Title = blog.Title,
                Description = blog.Description,
                Image = blog.Image,
                BlogCategoryId = blog.BlogCategoryId
            };

            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", model.BlogCategoryId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int id, BlogEditVM request)
        {
            if (id != request.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", request.BlogCategoryId);
                return View(request);
            }

            if (request.Photo != null)
            {
                if (!request.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Only image files are allowed");
                    var categories = await _categoryService.GetAllAsync();
                    ViewBag.Categories = new SelectList(categories, "Id", "Name", request.BlogCategoryId);
                    return View(request);
                }

                if (!request.Photo.CheckFileSize(2048))
                {
                    ModelState.AddModelError("Photo", "Max file size is 2MB");
                    var categories = await _categoryService.GetAllAsync();
                    ViewBag.Categories = new SelectList(categories, "Id", "Name", request.BlogCategoryId);
                    return View(request);
                }
            }

            await _blogService.EditAsync(request);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _blogService.DeleteAsync(id);
            return Ok();
        }
    }
}
