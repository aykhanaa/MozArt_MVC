using Final_MozArt.ViewModels.Order;

namespace Final_MozArt.Services.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderFromCookieAsync(string appUserId, string stripeSessionId, HttpContext httpContext);
        Task<List<OrderVM>> GetAllAsync();
        Task<List<OrderVM>> GetByUserIdAsync(string userId);
    }
}
