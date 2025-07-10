using Final_MozArt.Data;
using Final_MozArt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Final_MozArt.Areas.Admin.Controllers
{
    public class OrderController : MainController
    {
        private readonly IOrderService _orderService;
        private readonly AppDbContext _context;
        private readonly IHubContext<OrderHub> _hubContext; // SignalR hub əlavə et
        private const int PageSize = 4;

        public OrderController(IOrderService orderService, AppDbContext context, IHubContext<OrderHub> hubContext)
        {
            _orderService = orderService;
            _context = context;
            _hubContext = hubContext; // Inject et
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageCount = _orderService.GetPageCount(PageSize);
            if (page > pageCount) page = pageCount;
            if (page <= 0) page = 1;
            ViewBag.PageCount = pageCount;
            ViewBag.CurrentPage = page;
            var orders = await _orderService.GetPagedOrdersAsync(page, PageSize);
            return View(orders);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var order = await _orderService.GetOrderDetailAsync(id);
            if (order == null)
                return NotFound();
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Next(int id)
        {
            bool success = await _orderService.ToggleStatusAsync(id);
            if (!success)
                return NotFound();

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            // SignalR notification göndər
            await _hubContext.Clients.Group($"User_{order.AppUserId}")
                .SendAsync("ReceiveOrderStatusUpdate", id, order.Status, order.IsCanceled);

            return Json(new { success = true, id = id, status = order.Status, isCanceled = order.IsCanceled });
        }

        [HttpPost]
        public async Task<IActionResult> Prev(int id)
        {
            bool success = await _orderService.SetPreviousStatusAsync(id);
            if (!success)
                return NotFound();

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            // SignalR notification göndər
            await _hubContext.Clients.Group($"User_{order.AppUserId}")
                .SendAsync("ReceiveOrderStatusUpdate", id, order.Status, order.IsCanceled);

            return Json(new { success = true, id = id, status = order.Status, isCanceled = order.IsCanceled });
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrRepair(int id)
        {
            bool success = await _orderService.ToggleCancelStatusAsync(id);
            if (!success)
                return NotFound();

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            // SignalR notification göndər
            await _hubContext.Clients.Group($"User_{order.AppUserId}")
                .SendAsync("ReceiveOrderStatusUpdate", id, order.Status, order.IsCanceled);

            return Json(new { success = true, id = id, status = order.Status, isCanceled = order.IsCanceled });
        }
    }
}