using AutoMapper;
using Catalog.Api.DTOs;
using Catalog.Api.Entities;

namespace Catalog.Api.RequestHelper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(d => d.ProductBrand, o
                => o.MapFrom(s => s.ProductBrand.Name))
            .ForMember(d => d.ProductType, o
                => o.MapFrom(s => s.ProductType.Name));

        CreateMap<CreateProductDto, Product>();
    }
}