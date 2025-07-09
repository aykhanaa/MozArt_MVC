using Final_MozArt.Services;
using Final_MozArt.Services.Implementations;
using Final_MozArt.Services.Interfaces;

namespace Final_MozArt
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<ISliderService, SliderService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IInstagramService, InstagramService>();
            services.AddScoped<ISupportService, SupportService>();
            services.AddScoped<IAboutService, AboutService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<IAdvantageService, AdvantageService>();
            services.AddScoped<IContactIntroService, ContactIntroService>();
            services.AddScoped<IBlogCategoryService, BlogCategoryService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IColorService, ColorService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IContactMessageService, ContactMessageService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ILayoutService, LayoutService>();
            services.AddScoped<ISubscribeService, SubscribeService>();
            services.AddScoped<IBlogCommentService, BlogCommentService>();
            services.AddScoped<IProductCommentService, ProductCommentService>();
            services.AddScoped<IWishlistService, WishlistService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
