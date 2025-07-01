using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Blog;
using Final_MozArt.ViewModels.BlogComment;
using Final_MozArt.ViewModels.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Final_MozArt.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ISettingService _settingService;
        private readonly IBlogCommentService _blogCommentService;

        public BlogController(IBlogService blogService, ISettingService settingService, IBlogCommentService blogCommentService)
        {
            _blogService = blogService;
            _settingService = settingService;
            _blogCommentService = blogCommentService;
        }

        public async Task<IActionResult> Index()
        {
            var totalCount = await _blogService.GetTotalBlogCountAsync();
            var blogs = await _blogService.GetBlogsAsync(skip: 0, take: 3);
            var setting = _settingService.GetSettings();

            BlogUIVM model = new BlogUIVM
            {
                Blogs = blogs,
                TotalCount = totalCount,
                Setting = setting,
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LoadMore(int skip = 0, int take = 3)
        {
            try
            {
                var blogs = await _blogService.GetBlogsAsync(skip, take);

                var formattedBlogs = blogs.Select(b => new
                {
                    id = b.Id,
                    title = b.Title,
                    description = b.Description,
                    image = b.Image,
                    createDate = b.CreateDate,
                    blogCategoryName = b.BlogCategoryName 
                }).ToList();

                return Json(new { success = true, blogs = formattedBlogs });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoadMore Error: {ex.Message}");
                return Json(new { success = false, message = ex.Message, blogs = new List<object>() });
            }
        }



        public async Task<IActionResult> Detail(int id)
        {
            if (id == 0) return RedirectToAction("Index", "NotFound");

            var blogs = await _blogService.GetAllAsync();
            var blog = await _blogService.GetByIdAsync(id);
            if (blog == null) return RedirectToAction("Index", "NotFound");

            var setting = _settingService.GetSettings();
            var blogcomment = await _blogCommentService.GetAllByBlogIdAsync(blog.Id);
            BlogDetailVM model = new BlogDetailVM()
            {
                Blogs = blogs,
                Blog = blog,
                Setting = setting,
                BlogComments = blogcomment
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BlogCommentCreateVM commentVM)
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

                var success = await _blogCommentService.CreateAsync(commentVM, userId);

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





    }
}