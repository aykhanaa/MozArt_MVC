using Final_MozArt.Data;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Basket;
using Final_MozArt.ViewModels.Order;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Final_MozArt.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateOrderFromCookieAsync(string appUserId, string stripeSessionId, HttpContext httpContext)
        {
            var basketCookie = httpContext.Request.Cookies["basket"];
            if (string.IsNullOrEmpty(basketCookie))
                throw new Exception("Basket is empty");

            var basketItems = JsonConvert.DeserializeObject<List<BasketVM>>(basketCookie); // Qeyd: Artıq BasketDetailVM yox!
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
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToListAsync();

            return orders.Select(o => new OrderVM
            {
                Id = o.Id,
                AppUserEmail = o.AppUser?.Email, // istəyə görə əlavə et
                CreatedDate = o.CreateDate,
                TotalPrice = o.TotalPrice,
                Items = o.OrderItems.Select(i => new OrderItemVM
                {
                    ProductName = i.Product.Name,
                    Price = i.Price,
                    Quantity = i.Quantity
                }).ToList()
            }).ToList();
        }
    }
}
