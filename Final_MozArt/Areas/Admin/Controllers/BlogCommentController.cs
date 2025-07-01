using Final_MozArt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Final_MozArt.Areas.Admin.Controllers
{
    public class BlogCommentController : MainController
    {
        private readonly IBlogCommentService _blogCommentService;
        public BlogCommentController(IBlogCommentService blogCommentService)
        {
            _blogCommentService = blogCommentService;
        }

        [HttpGet]
        public async  Task<IActionResult> Index()
        {
            var comments = await _blogCommentService.GetAllAsync();
            return View(comments);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _blogCommentService.DeleteAsync(id);
                return Json(new { success = true, message = "Comment deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error while deleting comment." });
            }
        }

    }
}
