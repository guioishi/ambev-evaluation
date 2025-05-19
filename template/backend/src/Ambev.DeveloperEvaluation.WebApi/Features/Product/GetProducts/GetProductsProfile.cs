using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.GetProducts;

public class GetProductsProfile : Profile
{
    public GetProductsProfile()
    {
        CreateMap<GetProductsQueryResult, GetProductsQueryResponse>();
        CreateMap<GetProductsRequest, GetProductsQuery>();

        CreateMap<ProductListItemDto, GetProductsResponse>();
    }
}
