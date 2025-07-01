using Microsoft.AspNetCore.Mvc;

namespace Final_MozArt.Controllers
{
    public class NotFoundController : Controller
    {
        public IActionResult Index()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}
