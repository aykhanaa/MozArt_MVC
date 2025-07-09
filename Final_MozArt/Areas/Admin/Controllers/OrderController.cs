using Final_MozArt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Final_MozArt.Areas.Admin.Controllers
{
    public class OrderController : MainController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public async Task<IActionResult> Index()
        {
            var order = await _orderService.GetAllAsync();
            return View(order);
        }
    }
}
