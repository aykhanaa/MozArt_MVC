using Final_MozArt.Data;
using Final_MozArt.Helpers;
using Final_MozArt.Models;
using Final_MozArt.Services.Interfaces;
using Final_MozArt.ViewModels.Basket;
using Final_MozArt.ViewModels.Order;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Stripe.Checkout;

namespace Final_MozArt.Controllers
{
    public class PaymentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailSettings _emailSettings;
        private readonly IEmailService _emailService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;
        public PaymentController(AppDbContext context, UserManager<AppUser> userManager, IOptions<EmailSettings> emailSettings,IEmailService emailService, IBasketService basketService, IOrderService orderService)
        {
            _context = context;
            _userManager = userManager;
            _emailSettings = emailSettings.Value;
            _emailService = emailService;
            _basketService = basketService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Checkout()
        {
            return View();
        }

        //[HttpGet]
        //public async Task<IActionResult> CheckOut()
        //{
        //    var basketDetails = await _basketService.GetBasketDatasAsync(); // burdan gəlməlidir

        //    if (basketDetails == null || !basketDetails.Any())
        //    {
        //        return RedirectToAction("Index", "Basket");
        //    }

        //    var domain = "https://localhost:7286/";

        //    var options = new SessionCreateOptions
        //    {
        //        SuccessUrl = domain + "payment/success",
        //        CancelUrl = domain + "payment/failed",
        //        LineItems = new List<SessionLineItemOptions>(),
        //        Mode = "payment",
        //    };

        //    foreach (var item in basketDetails)
        //    {
        //        options.LineItems.Add(new SessionLineItemOptions
        //        {
        //            PriceData = new SessionLineItemPriceDataOptions
        //            {
        //                UnitAmount = (long)(item.Price * 100), // price * 100 cent
        //                Currency = "usd",
        //                ProductData = new SessionLineItemPriceDataProductDataOptions
        //                {
        //                    Name = item.Name
        //                }
        //            },
        //            Quantity = item.Count
        //        });
        //    }

        //    var service = new SessionService();
        //    Session session = service.Create(options);

        //    Response.Headers.Add("Location", session.Url);
        //    return new StatusCodeResult(303);
        //}

        [HttpGet]
        public async Task<IActionResult> CheckOut()
        {
            var basketDetails = await _basketService.GetBasketDatasAsync();

            if (basketDetails == null || !basketDetails.Any())
            {
                return RedirectToAction("Index", "Basket");
            }

            var domain = "https://localhost:7286/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + "payment/success?sessionId={CHECKOUT_SESSION_ID}", // buraya sessionId gələcək
                CancelUrl = domain + "payment/failed",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (var item in basketDetails)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Name
                        }
                    },
                    Quantity = item.Count
                });
            }

            var service = new SessionService();
            Session session = service.Create(options);

            // Redirect üçün 303 status və Location header istifadə olunur
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }










        //public async Task<IActionResult> Success()
        //{
        //    var basketCookie = Request.Cookies["basket"];

        //    if (string.IsNullOrEmpty(basketCookie))
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    var basketItems = JsonConvert.DeserializeObject<List<BasketDetailVM>>(basketCookie);

        //    Response.Cookies.Delete("basket");

        //    // Email göndərmə zamanı xəta baş verərsə, ödəniş prosesinə təsir etməsin
        //    try
        //    {
        //        await SendSuccessEmailAsync(basketItems);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the error but continue with success page
        //        // _logger.LogError(ex, "Email göndərmə zamanı xəta baş verdi");
        //    }

        //    return RedirectToAction("Index", "Home", new { payment = "success" });
        //}



        public async Task<IActionResult> Success(string sessionId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Burada artıq Stripe session ID əlində var
            await _orderService.CreateOrderFromCookieAsync(user.Id, sessionId, HttpContext);

            var basketCookie = Request.Cookies["basket"];
            if (!string.IsNullOrEmpty(basketCookie))
            {
                Response.Cookies.Delete("basket");
            }

            try
            {
                var basketItems = JsonConvert.DeserializeObject<List<BasketDetailVM>>(basketCookie);
                await SendSuccessEmailAsync(basketItems);
            }
            catch (Exception)
            {
                // Email error ignored
            }

            return RedirectToAction("Index", "Home", new { payment = "success" });
        }



        private async Task SendSuccessEmailAsync(List<BasketDetailVM> basketItems)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                Console.WriteLine("User tapılmadı - login olmayıb");
                return;
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                Console.WriteLine("User email-i yoxdur");
                return;
            }

            Console.WriteLine($"Email göndəriləcək: {user.Email}");

            var subject = "Ödəniş uğurla tamamlandı";

            string body = $@"
              <!DOCTYPE html>
              <html lang='az'>
              <head>
              <meta charset='UTF-8'>
              <meta name='viewport' content='width=device-width, initial-scale=1.0'>
              <title>Ödəniş Təsdiqi</title>
              <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f4f4f4;
                margin: 0;
                padding: 0;
            }}
            .email-container {{
                max-width: 600px;
                margin: 20px auto;
                background-color: #fff;
                padding: 20px;
                border-radius: 8px;
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            }}
            .email-header, .email-footer {{
                background-color: #4CAF50;
                color: white;
                text-align: center;
                padding: 10px;
            }}
            .email-body {{
                padding: 20px;
                color: #333;
            }}
            .email-body h2 {{
                color: #333;
            }}
            .product-details {{
                margin: 20px 0;
            }}
            .product-details table {{
                width: 100%;
                border-collapse: collapse;
            }}
            .product-details th, .product-details td {{
                border: 1px solid #ddd;
                padding: 8px;
                text-align: left;
            }}
            .product-details th {{
                background-color: #f2f2f2;
            }}
        </style>
    </head>
    <body>
        <div class='email-container'>
            <div class='email-header'>
                <h1>Ödəniş Təsdiqi</h1>
            </div>
            <div class='email-body'>
                <h2>Salam {user.UserName},</h2>
                <p>Sizin ödənişiniz uğurla tamamlandı. Alınan məhsullar aşağıdakı kimidir:</p>
                <div class='product-details'>
                    <table>
                        <thead>
                            <tr>
                                <th>Məhsul</th>
                                <th>Sayı</th>
                                <th>Cəmi</th>
                            </tr>
                        </thead>
                        <tbody>";
            var basketDetails = await _basketService.GetBasketDatasAsync(); // burdan gəlməlidir
            foreach (var item in basketDetails)
            {
                body += $@"
                            <tr>
                                <td>{item.Name}</td>
                                <td>{item.Count}</td>
                                <td>${item.Total:F2}</td>
                            </tr>";
            }

            body += @"
                        </tbody>
                    </table>
                </div>
                <p>Bizə güvəndiyiniz üçün təşəkkür edirik!</p>
            </div>
            <div class='email-footer'>
                <p>&copy; 2025 Final MozArt. Bütün hüquqlar qorunur.</p>
            </div>
        </div>
    </body>
    </html>";

            _emailService.Send(user.Email, subject, body);
        }
    }
}