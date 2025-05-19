using Ambev.DeveloperEvaluation.Application.Sales.CommonDTO;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;


public class GetSalesProfile : Profile
{
    public GetSalesProfile()
    {
        CreateMap<CustomerInfoDto, CustomerInfoResponse>();
        CreateMap<BranchInfoDto, BranchInfoResponse>();
        CreateMap<ProductSnapshotDto, ProductSnapshotResponse>();
        CreateMap<GetSaleProductDto, GetSaleItemResponse>();

        CreateMap<GetSaleRequest, GetSaleQuery>();
        CreateMap<GetSalesRequest, GetSalesQuery>();

        CreateMap<GetSaleResultDto, GetSaleResponse>()
            .ForMember(dest => dest.SaleNumber, opt => opt.MapFrom(src => src.SaleNumber))
            .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.SaleDate))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.IsCancelled))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<SaleResultDto, GetSalesResponse>()
            .ForMember(dest => dest.SaleNumber, opt => opt.MapFrom(src => src.SaleNumber))
            .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.SaleDate))
            .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch))
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.IsCancelled))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
    }
}
