using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace Final_MozArt
{
    [Authorize]  // Bu vacibdir - user authentication üçün
    public class OrderHub : Hub
    {
        public async Task JoinUserGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
        }

        public async Task SendOrderStatusUpdate(int orderId, bool newStatus)
        {
            await Clients.All.SendAsync("ReceiveOrderStatusUpdate", orderId, newStatus);
        }
    }
}