using Final_MozArt.Services.Implementations;
using Final_MozArt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Final_MozArt.Areas.Admin.Controllers
{
    public class ProductCommentController : MainController
    {
        private readonly IProductCommentService _productCommentService;

        public ProductCommentController(IProductCommentService productCommentService)
        {
            _productCommentService = productCommentService;
        }
        public async Task<IActionResult> Index()
        {
            var comments = await _productCommentService.GetAllAsync();
            return View(comments);  
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productCommentService.DeleteAsync(id);
                return Json(new { success = true, message = "Comment deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error while deleting comment." });
            }
        }

    }
}
