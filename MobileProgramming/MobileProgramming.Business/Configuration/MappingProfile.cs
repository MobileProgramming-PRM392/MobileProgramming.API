using AutoMapper;
using MobileProgramming.Business.Models.DTO.CartItems;
using MobileProgramming.Business.Models.DTO.Category;
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

            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<CartItem, CartItemDto>().ReverseMap();
            

            //user
            CreateMap<RegisterUserDto, User>().ReverseMap();
            CreateMap<User, UserInfoResponseDto>().ReverseMap();
        }
    }
}
