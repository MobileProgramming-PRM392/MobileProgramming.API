using AutoMapper;
using MobileProgramming.Business.Models.DTO;
using MobileProgramming.Business.Models.DTO.CartItems;
using MobileProgramming.Business.Models.DTO.Category;
using MobileProgramming.Business.Models.DTO.Chat;
using MobileProgramming.Business.Models.DTO.Feedbacks;
using MobileProgramming.Business.Models.DTO.Order;
using MobileProgramming.Business.Models.DTO.Product;
using MobileProgramming.Business.Models.DTO.User;
using MobileProgramming.Business.Models.DTO.User.ResponseDto;
using MobileProgramming.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Business.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDisplayDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src =>
                    src.ProductImages.FirstOrDefault().ImageUrl))
                .ReverseMap();

            CreateMap<ProductDetailDto, Product>().ReverseMap();
            CreateMap<CreateProductDto, Product>().ReverseMap();


            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<CartItem, CartItemDto>().ReverseMap();
            CreateMap<Cart, CartDto>().ReverseMap();
            

            //user
            CreateMap<RegisterUserDto, User>().ReverseMap();
            CreateMap<User, UserInfoResponseDto>().ReverseMap();
            CreateMap<User, UserInfoDto>().ReverseMap();

            //chat
            CreateMap<SendMessageDto, ChatMessage>().ReverseMap();

            CreateMap<Order, OrderDto>();


            //Fêdback
            CreateMap<Feedback, FeedbackDtoResponse>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
                .ReverseMap();    


            

            // Map Cart entity to CartDto
            CreateMap<Cart, CartOrderDto>();

            // Map CartItem entity to CartItemDto
            CreateMap<CartItem, CartOrderItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Product.BriefDescription))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price));

            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Order.OrderId))
                .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.Order.CartId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Order.UserId))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.Order.PaymentMethod))
                .ForMember(dest => dest.BillingAddress, opt => opt.MapFrom(src => src.Order.BillingAddress))
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.Order.OrderStatus))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.Order.OrderDate))
                .ForMember(dest => dest.Cart, opt => opt.MapFrom(src => src.Order.Cart));
        }
    }
}
