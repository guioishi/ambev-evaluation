namespace Ambev.DeveloperEvaluation.WebApi.Features.Product.GetProduct;

public class GetProductRequest
{
    public Guid ProductId { get; }

    public GetProductRequest(Guid productId)
    {
        ProductId = productId;
    }
}
