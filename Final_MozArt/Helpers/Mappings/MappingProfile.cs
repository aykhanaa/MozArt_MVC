using AutoMapper;
using Final_MozArt.Models;
using Final_MozArt.ViewModels.About;
using Final_MozArt.ViewModels.Advantage;
using Final_MozArt.ViewModels.Blog;
using Final_MozArt.ViewModels.BlogCategory;
using Final_MozArt.ViewModels.BlogComment;
using Final_MozArt.ViewModels.Brand;
using Final_MozArt.ViewModels.Category;
using Final_MozArt.ViewModels.Color;
using Final_MozArt.ViewModels.ContactIntro;
using Final_MozArt.ViewModels.ContactMessage;
using Final_MozArt.ViewModels.Instagram;
using Final_MozArt.ViewModels.Product;
using Final_MozArt.ViewModels.ProductComment;
using Final_MozArt.ViewModels.Setting;
using Final_MozArt.ViewModels.Slider;
using Final_MozArt.ViewModels.Support;
using Final_MozArt.ViewModels.Tag;
using Final_MozArt.ViewModels.Video;

namespace Final_MozArt.Helpers.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Slider, SliderVM>();
            CreateMap<SliderCreateVM, Slider>();
            CreateMap<SliderEditVM, Slider>().ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<Category, CategoryVM>();
            CreateMap<CategoryCreateVM, Category>();
            CreateMap<CategoryVM, CategoryEditVM>();

            CreateMap<Brand, BrandVM>();
            CreateMap<BrandCreateVM, Brand>();
            CreateMap<BrandVM, BrandEditVM>();

            CreateMap<Instagram, InstagramVM>();
            CreateMap<InstagramCreateVM, Instagram>();
            CreateMap<InstagramVM, InstagramEditVM>();

            CreateMap<Support, SupportVM>();
            CreateMap<SupportCreateVM, Support>();
            CreateMap<SupportVM, SupportEditVM>();

            CreateMap<About, AboutVM>();
            CreateMap<AboutCreateVM, About>();
            CreateMap<AboutVM, AboutEditVM>();

            CreateMap<Setting, SettingVM>();
            CreateMap<SettingEditVM, Setting>();

            CreateMap<Video, VideoVM>();
            CreateMap<VideoCreateVM, Video>();
            CreateMap<VideoVM, VideoEditVM>();
                
            CreateMap<Advantage, AdvantageVM>();
            CreateMap<AdvantageCreateVM, Advantage>();
            CreateMap<AdvantageVM, AdvantageEditVM>();

            CreateMap<ContactIntro, ContactIntroVM>();
            CreateMap<ContactIntroCreateVM, ContactIntro>();
            CreateMap<ContactIntroVM, ContactIntroEditVM>();

            CreateMap<BlogCategory, BlogCategoryVM>();
            CreateMap<BlogCategoryCreateVM, BlogCategory>();
            CreateMap<BlogCategoryVM, BlogCategoryEditVM>();

            CreateMap<Blog, BlogVM>()
              .ForMember(dest => dest.BlogCategoryName, opt => opt.MapFrom(src => src.BlogCategory.Name));

            CreateMap<BlogCreateVM, Blog>();
            CreateMap<BlogVM, BlogEditVM>();

            CreateMap<Tag, TagVM>();
            CreateMap<TagCreateVM, Tag>();
            CreateMap<TagVM, TagEditVM>();

            CreateMap<Color, ColorVM>();
            CreateMap<ColorCreateVM, Color>();
            CreateMap<ColorVM, ColorEditVM>();


            CreateMap<Product, ProductDetailVM>()
    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
    .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
    .ForMember(dest => dest.ColorNames, opt => opt.MapFrom(src => src.ProductColors.Select(pc => pc.Color.Name)))
    .ForMember(dest => dest.TagNames, opt => opt.MapFrom(src => src.ProductTags.Select(pt => pt.Tag.Name)))
    .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

            CreateMap<ProductCreateVM, Product>()
                .ForMember(dest => dest.ProductColors, opt => opt.Ignore())
                .ForMember(dest => dest.ProductTags, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<ProductEditVM, Product>()
                .ForMember(dest => dest.ProductColors, opt => opt.Ignore())
                .ForMember(dest => dest.ProductTags, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<Product, ProductEditVM>()
                .ForMember(dest => dest.ColorIds, opt => opt.MapFrom(src => src.ProductColors.Select(pc => pc.ColorId)))
                .ForMember(dest => dest.TagIds, opt => opt.MapFrom(src => src.ProductTags.Select(pt => pt.TagId)))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Photos, opt => opt.Ignore());

            CreateMap<Product, ProductVM>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Images.FirstOrDefault(i => i.IsMain).Image))
                .ForMember(dest => dest.Hower, opt => opt.MapFrom(src => src.Images.FirstOrDefault(i => i.IsHover).Image))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate));


            CreateMap<ProductImage, SetIsMainVM>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.Id));

            CreateMap<ContactMessage, ContactMessageVM>();
            CreateMap<ContactMessageCreateVM, ContactMessage>();


            CreateMap<BlogComment, BlogCommentVM>();
            CreateMap<BlogCommentCreateVM, BlogComment>();

            CreateMap<ProductComment, ProductCommentVM>();
            CreateMap<ProductCommentCreateVM, ProductComment>();

        }
    }
}
