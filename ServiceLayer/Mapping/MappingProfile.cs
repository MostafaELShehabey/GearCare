using AutoMapper;
using DomainLayer.Dto;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile() {

            CreateMap<ApplicationUserDto, ApplicationUser>().ReverseMap();
            CreateMap<CompleteUserData, ApplicationUser>().ReverseMap();
            CreateMap<CompleteSelerData, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUser, ServiceProvideroutDTO>().ReverseMap();

            CreateMap<RepareOrderDto, RepareOrder>().ReverseMap();
            CreateMap<RepareOrderToAccept, RepareOrder>().ReverseMap();

            CreateMap<CategoryDto,Category>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductOutputDTO>().ReverseMap();

            CreateMap<WinchDto, Winch>().ReverseMap();
            CreateMap<WinchOrder, WinchOrderDTO>().ReverseMap();

            CreateMap<DiscountDto, Discount>().ForMember(des => des.Product, o => o.Ignore());
            CreateMap<Discount, DiscountDto>();
          

            CreateMap<Product_Shoppingcart, ProductDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.price));

            CreateMap<ProductDto, Product_Shoppingcart>();
            CreateMap<ApplicationUser, Service_ProviderDto>().ReverseMap();

        }
    }
}
