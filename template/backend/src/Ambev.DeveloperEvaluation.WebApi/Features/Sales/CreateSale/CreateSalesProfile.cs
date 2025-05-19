using Ambev.DeveloperEvaluation.Application.Sales.CommonDTO;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSalesProfile : Profile
{
    public CreateSalesProfile()
    {
        CreateMap<CreateSaleRequest, CreateSaleCommand>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src =>
                src.Items.Select(i => new SaleProductDto(
                    i.ProductId,
                    i.Quantity))));

        CreateMap<SaleProductRequest, SaleProductDto>()
            .ConstructUsing(src => new SaleProductDto(
                src.ProductId,
                src.Quantity));

        CreateMap<SaleResultDto, CreateSaleResponse>();
        CreateMap<GetSaleProductDto, SaleItemResponse>();
    }
}
