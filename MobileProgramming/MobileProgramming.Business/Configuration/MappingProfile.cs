using AutoMapper;
using MobileProgramming.Business.Models.DTO.Product;
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

            CreateMap<ProductDetailDto, Product>() 
                .ReverseMap();
        }
    }
}
