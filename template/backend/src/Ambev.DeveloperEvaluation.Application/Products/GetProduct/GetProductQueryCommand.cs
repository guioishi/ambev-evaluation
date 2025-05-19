using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

public class GetProductQueryCommand : IRequest<GetProductResultDto>
{
    public Guid ProductId { get; set; }
}
