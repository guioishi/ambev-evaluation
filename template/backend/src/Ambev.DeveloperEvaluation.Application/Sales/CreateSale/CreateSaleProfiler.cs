using Ambev.DeveloperEvaluation.Application.Sales.CommonDTO;
using Ambev.DeveloperEvaluation.Domain.Entities.Sale;
using Ambev.DeveloperEvaluation.Domain.Enums;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleProfiler : Profile
{
    public CreateSaleProfiler()
    {
        CreateMap<CustomerInfoDto, CustomerInfo>()
            .ForMember(dest => dest.Category,
                opt => opt.MapFrom(src =>
                    Enum.Parse<UserRole>(src.Category)));

        CreateMap<Sale, SaleResultDto>()
            .ForMember(dest => dest.SaleNumber, opt => opt.MapFrom(src => src.SaleNumber))
            .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.SaleDate))
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.IsCancelled))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.SaleProducts));

        CreateMap<SaleProduct, GetSaleProductDto>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
    }
}
