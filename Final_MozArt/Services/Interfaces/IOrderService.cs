using Final_MozArt.Models;
using Final_MozArt.ViewModels.Order;

namespace Final_MozArt.Services.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderFromCookieAsync(string appUserId, string stripeSessionId, HttpContext httpContext);
        Task<List<OrderVM>> GetAllAsync();
        Task<List<OrderVM>> GetByUserIdAsync(string userId);

        Task<List<Order>> GetPagedOrdersAsync(int page, int pageSize);
        int GetPageCount(int pageSize);
        Task<Order?> GetOrderDetailAsync(int id);
        Task<bool> ToggleStatusAsync(int id);
        Task<bool> SetPreviousStatusAsync(int id);
        Task<bool> ToggleCancelStatusAsync(int id);
        Task UpdateOrderStatusAsync(int orderId, bool newStatus);



    }
}
