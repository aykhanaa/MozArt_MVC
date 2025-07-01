using Microsoft.AspNetCore.Mvc;

namespace Final_MozArt.Controllers
{
    public class UnauthorizedController : Controller
    {
        public IActionResult Index()
        {
            Response.StatusCode = 401;
            return View();
        }
    }
}
