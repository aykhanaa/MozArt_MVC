using Final_MozArt.Data;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Basket;
using Final_MozArt.ViewModels.Order;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Final_MozArt.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<OrderHub> _orderHubContext;

        public OrderService(AppDbContext context, IHubContext<OrderHub> orderHubContext)
        {
            _context = context;
            _orderHubContext = orderHubContext;
        }

        public async Task CreateOrderFromCookieAsync(string appUserId, string stripeSessionId, HttpContext httpContext)
        {
            var basketCookie = httpContext.Request.Cookies["basket"];
            if (string.IsNullOrEmpty(basketCookie))
                throw new Exception("Basket is empty");

            var basketItems = JsonConvert.DeserializeObject<List<BasketVM>>(basketCookie);
            if (basketItems == null || !basketItems.Any())
                throw new Exception("Basket is empty");

            var productIds = basketItems.Select(x => x.Id).ToList();
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            if (products.Count != productIds.Count)
                throw new Exception("Some products were not found");

            var orderItems = new List<OrderItem>();
            decimal totalPrice = 0;

            foreach (var basketItem in basketItems)
            {
                var product = products.FirstOrDefault(p => p.Id == basketItem.Id);
                if (product == null)
                    continue;

                var itemTotal = product.Price * basketItem.Count;
                totalPrice += itemTotal;

                orderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = basketItem.Count,
                    Price = product.Price
                });
            }

            var order = new Order
            {
                AppUserId = appUserId,
                CreateDate = DateTime.UtcNow,
                StripeId = stripeSessionId,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            httpContext.Response.Cookies.Delete("basket");
        }

        public async Task<List<OrderVM>> GetAllAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.AppUser)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToListAsync();

            return orders.Select(o => new OrderVM
            {
                Id = o.Id,
                AppUserEmail = o.AppUser?.Email,
                CreatedDate = o.CreateDate,
                TotalPrice = o.TotalPrice,
                Status = o.Status, // Bu əlavə edildi
                IsCanceled = o.IsCanceled, // Bu da əlavə edildi
                AppUserId = o.AppUserId,
                Items = o.OrderItems.Select(i => new OrderItemVM
                {
                    ProductName = i.Product.Name,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList()
            }).ToList();
        }

        public async Task<List<OrderVM>> GetByUserIdAsync(string userId)
        {
            var orders = await _context.Orders
                .Where(o => o.AppUserId == userId)
                .Include(o => o.AppUser) // Bu əlavə edildi
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToListAsync();

            return orders.Select(o => new OrderVM
            {
                Id = o.Id,
                AppUserEmail = o.AppUser?.Email,
                CreatedDate = o.CreateDate,
                TotalPrice = o.TotalPrice,
                Status = o.Status, // Bu əlavə edildi
                IsCanceled = o.IsCanceled, // Bu da əlavə edildi
                AppUserId = o.AppUserId,
                Items = o.OrderItems.Select(i => new OrderItemVM
                {
                    ProductName = i.Product.Name,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList()
            }).ToList();
        }

        public async Task<List<Order>> GetPagedOrdersAsync(int page, int pageSize)
        {
            return await _context.Orders
                .OrderByDescending(x => x.CreateDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.AppUser)
                .ToListAsync();
        }

        public int GetPageCount(int pageSize)
        {
            int count = _context.Orders.Count();
            return Math.Max(1, (int)Math.Ceiling((decimal)count / pageSize));
        }

        public async Task<Order?> GetOrderDetailAsync(int id)
        {
            return await _context.Orders
                .Include(x => x.OrderItems)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Images)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ToggleStatusAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            order.Status = order.Status == false ? null : true;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SetPreviousStatusAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null || order.IsCanceled) return false;

            order.Status = order.Status == true ? null : false;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ToggleCancelStatusAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            order.IsCanceled = !order.IsCanceled;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task UpdateOrderStatusAsync(int orderId, bool newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = newStatus;
                await _context.SaveChangesAsync();

                // Status yeniləndikdən sonra istifadəçiyə bildiriş göndər
                await _orderHubContext.Clients.User(order.AppUserId).SendAsync("ReceiveOrderStatusUpdate", orderId, newStatus);
            }
        }
    }
}
