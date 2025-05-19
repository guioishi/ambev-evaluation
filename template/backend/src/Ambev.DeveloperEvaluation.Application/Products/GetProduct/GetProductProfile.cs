using Ambev.DeveloperEvaluation.Domain.Entities.Product;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductProfile : Profile
{
    public GetProductProfile()
    {
        CreateMap<Product, GetProductResultDto>();
        CreateMap<ProductRating, ProductRatingDto>();
    }
}
